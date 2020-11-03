using System;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.App.Events.ProductCatalog;

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

        [Topic("pubsub", "products-sync")]
        [HttpPost("products-sync")]
        public async Task SubscribeProductsSync(ProductListReplicated @event)
        {
            _logger.LogInformation($"Received data for products-sync: {@event.Products.Count} products.");

            await _daprClient.SaveStateAsync("statestore", "products", @event);
        }
    }
}
