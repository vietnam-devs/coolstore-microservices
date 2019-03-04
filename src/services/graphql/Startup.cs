using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Template.Rest.Standard;
using VND.CoolStore.Services.GraphQL.v1;
using VND.CoolStore.Services.GraphQL.v1.Types;
using static VND.CoolStore.Services.Cart.v1.Grpc.CartService;
using static VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;
using static VND.CoolStore.Services.Review.v1.Grpc.ReviewService;
using static VND.CoolStore.Services.Review.v1.Grpc.PingService;
using static VND.CoolStore.Services.Catalog.v1.Grpc.CatalogService;
using static VND.CoolStore.Services.Rating.v1.Grpc.RatingService;

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
                var config = resolver.GetService<IConfiguration>();

                var cartChannel = new Channel(config["RpcClients:CartService"], ChannelCredentials.Insecure);
                var cartClient = new CartServiceClient(cartChannel);

                var inventoryChannel = new Channel(config["RpcClients:InventoryService"], ChannelCredentials.Insecure);
                var inventoryClient = new InventoryServiceClient(inventoryChannel);

                var reviewChannel = new Channel(config["RpcClients:ReviewService"], ChannelCredentials.Insecure);
                var reviewClient = new ReviewServiceClient(reviewChannel);
                var pingClient = new PingServiceClient(reviewChannel);

                var catalogChannel = new Channel(config["RpcClients:CatalogService"], ChannelCredentials.Insecure);
                var catalogClient = new CatalogServiceClient(catalogChannel);

                var ratingChannel = new Channel(config["RpcClients:RatingService"], ChannelCredentials.Insecure);
                var ratingClient = new RatingServiceClient(ratingChannel);

                services.AddSingleton(typeof(CartServiceClient), cartClient);
                services.AddSingleton(typeof(InventoryServiceClient), inventoryClient);
                services.AddSingleton(typeof(ReviewServiceClient), reviewClient);
                services.AddSingleton(typeof(PingServiceClient), pingClient);
                services.AddSingleton(typeof(CatalogServiceClient), catalogClient);
                services.AddSingleton(typeof(RatingServiceClient), ratingClient);

                services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

                services.AddSingleton<CoolStoreQuery>();
                services.AddSingleton<CoolStoreMutation>();
                services.AddSingleton<CoolStoreSchema>();

                services.AddSingleton<SampleType>();

                services.AddGraphQL(options =>
                    {
                        options.EnableMetrics = true;
                        options.ExposeExceptions = Environment.IsDevelopment();
                    })
                    .AddDataLoader();

                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseGraphQL<CoolStoreSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions()
            {
                GraphQLEndPoint = "/graphql",
                Path = "/playground"
            });
            app.UseGraphiQLServer(new GraphiQLOptions
            {
                GraphiQLPath = "/graphiql",
                GraphQLEndPoint = "/graphql"
            });
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions()
            {
                Path = "/voyager",
                GraphQLEndPoint = "/graphql"
            });

            app.UseStandardTemplate();
        }
    }
}
