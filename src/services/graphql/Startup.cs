using System.Threading.Tasks;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.AspNetCore.Configuration;
using NetCoreKit.Template.Rest.Standard;
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
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStandardTemplate((svc, resolver) =>
            {
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
                            /* https://github.com/aspnet/AspNetCore/issues/4457 */
                            .SetIsOriginAllowed(host => true) 
                            .AllowCredentials());
                });

                services.AddSignalR(options => options.EnableDetailedErrors = true)
                    .AddQueryStreamHubWithTracing();

                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            var basePath = Configuration.GetBasePath();

            if (basePath.EndsWith("/"))
            {
                basePath = basePath.Substring(0, basePath.Length - 1);
            }

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseWebSockets();

            app.UseSignalR(routes => { routes.MapHub<QueryStreamHub>(new PathString($"{basePath}/graphql")); });

            app.UseGraphiQLServer(new GraphiQLOptions
            {
                GraphiQLPath = $"{basePath}/graphiql",
                GraphQLEndPoint = $"{basePath}/api/graphql"
            });

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions()
            {
                GraphQLEndPoint = $"{basePath}/api/graphql",
                Path = $"{basePath}/playground"
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
                    context.Response.Redirect($"{basePath}/graphiql");
                    return Task.CompletedTask;
                });
            });

            app.UseStandardTemplate();
        }
    }
}
