using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Template.Rest.Standard;
using MyCartService = VND.CoolStore.Services.Cart.v1.Grpc.CartService;
using MyInventoryService = VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;
using MyReviewService = review.ReviewService;
using MyPingService = review.PingService;

namespace VND.CoolStore.Services.WebAggregator
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStandardTemplate((svc, resolver) =>
            {
                var config = resolver.GetService<IConfiguration>();

                var cartChannel = new Channel(config["RpcClients:CartService"], ChannelCredentials.Insecure);
                var cartClient = new MyCartService.CartServiceClient(cartChannel);

                var inventoryChannel = new Channel(config["RpcClients:InventoryService"], ChannelCredentials.Insecure);
                var inventoryClient = new MyInventoryService.InventoryServiceClient(inventoryChannel);

                var reviewChannel = new Channel(config["RpcClients:ReviewService"], ChannelCredentials.Insecure);
                var reviewClient = new MyReviewService.ReviewServiceClient(reviewChannel);

                var pingClient = new MyPingService.PingServiceClient(reviewChannel);

                services.AddSingleton(typeof(MyCartService.CartServiceClient), cartClient);
                services.AddSingleton(typeof(MyInventoryService.InventoryServiceClient), inventoryClient);
                services.AddSingleton(typeof(MyReviewService.ReviewServiceClient), reviewClient);
                services.AddSingleton(typeof(MyPingService.PingServiceClient), pingClient);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStandardTemplate();
        }
    }
}
