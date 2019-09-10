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

namespace VND.CoolStore.Services.GraphQL
{
  public class Startup
  {
    public static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());

    public Startup(IConfiguration configuration, IHostingEnvironment environment)
    {
      Configuration = configuration;
      Environment = environment;

      //if (Environment.IsDevelopment())
      {
        IdentityModelEventSource.ShowPII = true;
      }
    }

    public IConfiguration Configuration { get; }
    public IHostingEnvironment Environment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        var cors = Configuration.GetValue<string>("Cors:Origins").Split(',');
        options.AddPolicy("CorsPolicy",
                  policy =>
                  {
                    var builder = policy
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials()
                              .WithOrigins(cors)
                              /* https://github.com/aspnet/AspNetCore/issues/4457 */
                              .SetIsOriginAllowed(host => true);
                  });
      });

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      RegisterGrpcServices(services);
      RegisterGraphQl(services);
      RegisterAuth(services);

      services
          .AddSignalR(options => options.EnableDetailedErrors = true)
          .AddQueryStreamHubWithTracing();
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

      app.UseSignalR(routes => { routes.MapHub<QueryStreamHub>(new PathString($"{basePath}/graphql")); });
      UseDevTools(app, basePath);
    }

    private static void UseDevTools(IApplicationBuilder app, string basePath)
    {
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
                context.Response.Redirect($"{basePath}/voyager");
                return Task.CompletedTask;
              });
      });
    }

    private static void RegisterGraphQl(IServiceCollection services)
    {
      services.AddSingleton<ICoolStoreResolverService, CoolStoreResolverService>();
      services.AddSingleton<CoolStoreSchema>();
      services.AddSingleton(provider => provider.GetRequiredService<CoolStoreSchema>().CoolStore);
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    private void RegisterGrpcServices(IServiceCollection services)
    {
      services.AddSingleton(
          typeof(CartServiceClient),
          RegisterGrpcService<CartServiceClient>("CartEndPoint"));

      services.AddSingleton(
          typeof(InventoryServiceClient),
          RegisterGrpcService<InventoryServiceClient>("InventoryEndPoint"));

      services.AddSingleton(
          typeof(CatalogServiceClient),
          RegisterGrpcService<CatalogServiceClient>("CatalogEndPoint"));

      services.AddSingleton(
          typeof(RatingServiceClient),
          RegisterGrpcService<RatingServiceClient>("RatingEndPoint"));
    }

    private TService RegisterGrpcService<TService>(string serviceName)
        where TService : ClientBase<TService>
    {
      var rpcClients = Configuration.GetSection("GrpcEndPoints");
      var channel = new Channel(rpcClients[serviceName], ChannelCredentials.Insecure);
      var client = (TService)typeof(TService)
          .GetConstructor(new[] { typeof(Channel) })
          .Invoke(new object[] { channel });

      return client;
    }

    private void RegisterAuth(IServiceCollection services)
    {
      services
          .AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          .AddJwtBearer(options =>
          {
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

            options.Authority = Configuration["Idp:Authority"];
            options.Audience = Configuration["Idp:Audience"];
            options.RequireHttpsMetadata = false; // for demo only

            options.Events = new JwtBearerEvents
            {
              OnTokenValidated = context =>
                    {
                      // this will help us to authentication inside schema authorized
                      context.HttpContext.User = context.Principal;
                      return Task.CompletedTask;
                    },
              OnMessageReceived = context =>
                    {
                      var accessToken = context.Request.Query["access_token"].ToString();
                      var path = context.HttpContext.Request.Path;

                      if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/graphql")))
                      {
                        context.Token = accessToken;
                      }

                      return Task.CompletedTask;
                    }
            };
          });
    }
  }
}
