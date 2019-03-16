using System;
using System.Threading.Tasks;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
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
        public static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());

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

            // https://github.com/aspnet/Docs/blob/master/aspnetcore/signalr/authn-and-authz/sample/Startup.cs
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    // Configure JWT Bearer Auth to expect our security key
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                            ValidateAudience = false,
                            ValidateIssuer = false,
                            ValidateActor = false,
                            ValidateLifetime = true,
                            IssuerSigningKey = SecurityKey,
                        };

                    options.Authority = Configuration["AuthN:Authority"];
                    options.Audience = Configuration["AuthN:Audience"];
                    options.RequireHttpsMetadata = false; // for demo only

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("graphql")))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy => policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowCredentials()
                        /* https://github.com/aspnet/AspNetCore/issues/4457 */
                        .SetIsOriginAllowed(host => true)
                );
            });

            services.AddSignalR(options => options.EnableDetailedErrors = true)
                .AddQueryStreamHubWithTracing();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app)
        {
            var basePath = Configuration["Hosts:BasePath"];
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

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();

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
        }
    }
}
