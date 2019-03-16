using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Grpc.Core;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NetCoreKit.Infrastructure.AspNetCore.Configuration;
using NetCoreKit.Utils.Extensions;
using tanka.graphql.server;
using VND.CoolStore.Services.GraphQL.v1;
using static VND.CoolStore.Services.Cart.v1.Grpc.CartService;
using static VND.CoolStore.Services.Catalog.v1.Grpc.CatalogService;
using static VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;
using static VND.CoolStore.Services.Rating.v1.Grpc.RatingService;
using static VND.CoolStore.Services.Review.v1.Grpc.PingService;
using static VND.CoolStore.Services.Review.v1.Grpc.ReviewService;

namespace VND.CoolStore.Services.GraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            if (Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var cartChannel = new Channel(Configuration["RpcClients:CartService"], ChannelCredentials.Insecure);
            var cartClient = new CartServiceClient(cartChannel);

            var inventoryChannel = new Channel(Configuration["RpcClients:InventoryService"],
                ChannelCredentials.Insecure);
            var inventoryClient = new InventoryServiceClient(inventoryChannel);

            var reviewChannel = new Channel(Configuration["RpcClients:ReviewService"], ChannelCredentials.Insecure);
            var reviewClient = new ReviewServiceClient(reviewChannel);
            var pingClient = new PingServiceClient(reviewChannel);

            var catalogChannel =
                new Channel(Configuration["RpcClients:CatalogService"], ChannelCredentials.Insecure);
            var catalogClient = new CatalogServiceClient(catalogChannel);

            var ratingChannel = new Channel(Configuration["RpcClients:RatingService"], ChannelCredentials.Insecure);
            var ratingClient = new RatingServiceClient(ratingChannel);

            services.AddSingleton(typeof(CartServiceClient), cartClient);
            services.AddSingleton(typeof(InventoryServiceClient), inventoryClient);
            services.AddSingleton(typeof(ReviewServiceClient), reviewClient);
            services.AddSingleton(typeof(PingServiceClient), pingClient);
            services.AddSingleton(typeof(CatalogServiceClient), catalogClient);
            services.AddSingleton(typeof(RatingServiceClient), ratingClient);

            services.AddSingleton<ICoolStoreResolverService, CoolStoreResolverService>();
            services.AddSingleton<CoolStoreSchema>();
            services.AddSingleton(provider => provider.GetRequiredService<CoolStoreSchema>().CoolStore);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy => policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowCredentials()
                        .SetIsOriginAllowed(host => true) /* https://github.com/aspnet/AspNetCore/issues/4457 */
                );
            });

            services.AddSignalR(options => options.EnableDetailedErrors = true)
                .AddQueryStreamHubWithTracing();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app)
        {
            var basePath = Configuration.GetBasePath();
            basePath = basePath.EndsWith('/') ? basePath.TrimEnd('/') : basePath;

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseWebSockets();

            app.UseSignalR(routes => { routes.MapHub<QueryStreamHub>(new PathString($"{basePath}/graphql")); });

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions()
            {
                Path = $"{basePath}/playground",
                GraphQLEndPoint = $"{basePath}/api/graphql"
            });

            app.UseGraphQLVoyager(new GraphQLVoyagerOptions()
            {
                Path = $"{basePath}/voyager",
                GraphQLEndPoint = $"{basePath}/api/graphql"
            });

            app.UseRouter(builder =>
            {
                builder.MapGet(basePath, context =>
                {
                    context.Response.Redirect($"{basePath}/playground");
                    return Task.CompletedTask;
                });
            });
            
            app.UseMvc();
        }
    }

    internal class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration config)
        {
            var displayUrl = context.Request.GetDisplayUrl().ToLower();

            if (displayUrl.Contains("/api/graphql"))
            {
                var authorizedKeyValue = context.Request.Headers.FirstOrDefault(k => k.Key.ToLowerInvariant() == "authorization");
                if (!string.IsNullOrEmpty(authorizedKeyValue.Value))
                {
                    var authority = config["AuthN:Authority"];
                    var audience = config["AuthN:Audience"];

                    var token = authorizedKeyValue.Value.ToString();

                    var client = new HttpClient();
                    var disco = await client.GetDiscoveryDocumentAsync(authority);
                    if (disco.IsError)
                    {
                        throw new AuthenticationException($"Could not discovery the OAuth Server at {authority}");
                    }

                    var keys = new List<SecurityKey>();

                    foreach (var webKey in disco.KeySet.Keys)
                    {
                        var e = Base64Url.Decode(webKey.E);
                        var n = Base64Url.Decode(webKey.N);

                        var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                        {
                            KeyId = webKey.Kid
                        };

                        keys.Add(key);
                    }

                    var parameters = new TokenValidationParameters
                    {
                        ValidIssuer = disco.Issuer,
                        ValidAudience = audience,
                        IssuerSigningKeys = keys,

                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,

                        RequireSignedTokens = true,
                        ValidateLifetime = true,
                        ValidateIssuer = false
                    };

                    var handler = new JwtSecurityTokenHandler();
                    handler.InboundClaimTypeMap.Clear();

                    var user = handler.ValidateToken(token.TrimStart("bearer").TrimStart(" "), parameters, out _);
                    if (user != null)
                    {
                        context.User = user; // for internal usage
                    }
                }
            }

            await _next.Invoke(context);
        }
    }
}
