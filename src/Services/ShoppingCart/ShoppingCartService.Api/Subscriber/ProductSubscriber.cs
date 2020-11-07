using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Events.ProductCatalog;
using N8T.Infrastructure.App.Requests.ProductCatalog;

namespace ShoppingCartService.Api.Subscriber
{
    [ApiController]
    [Route("")]
    public class ProductSubscriber : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ProductSubscriber> _logger;

        public ProductSubscriber(DaprClient daprClient, ILogger<ProductSubscriber> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //TODO: for reference only
        [Topic("pubsub", "products-sync")]
        [HttpPost("products-sync")]
        public async Task SubscribeProductsSync(ProductListReplicated @event)
        {
            _logger.LogInformation($"Received data for products-sync: {@event.ProductIds.Count} product ids.");

            var data = new ProductByIdsRequest {ProductIds = @event.ProductIds};
            var products = await _daprClient.InvokeMethodAsync<ProductByIdsRequest, List<ProductDto>>(
                "productcatalogapp", "get-products-by-ids", data);

            await _daprClient.SaveStateAsync("statestore", "products", products);
            _logger.LogInformation("Finished save products to dapr state");

            // make it easy for looking up by index
            foreach (var product in products)
            {
                await _daprClient.SaveStateAsync("statestore", $"product-{product.Id}", product);
            }
            _logger.LogInformation("Finished save product by key to dapr state");
        }
    }
}
