using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Template.Rest.Standard;
using MyInventoryService = VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;
using MyReviewService = VND.CoolStore.Services.Review.v1.Grpc.ReviewService;

namespace VND.CoolStore.Services.WebAggregator
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStandardTemplate((svc, resolver) =>
            {
                var config = resolver.GetService<IConfiguration>();

                var inventoryChannel = new Channel(config["RpcClients:InventoryService"], ChannelCredentials.Insecure);
                var inventoryClient = new MyInventoryService.InventoryServiceClient(inventoryChannel);

                var reviewChannel = new Channel(config["RpcClients:ReviewService"], ChannelCredentials.Insecure);
                var reviewClient = new MyReviewService.ReviewServiceClient(reviewChannel);

                services.AddSingleton(typeof(MyInventoryService.InventoryServiceClient), inventoryClient);
                services.AddSingleton(typeof(MyReviewService.ReviewServiceClient), reviewClient);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStandardTemplate();
        }
    }
}
