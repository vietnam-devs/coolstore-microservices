using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Requests.Inventory;
using ShoppingCartService.Domain.Gateway;

namespace ShoppingCartService.Infrastructure.Gateway
{
    public class InventoryGateway : IInventoryGateway 
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<InventoryGateway> _logger;

        public InventoryGateway(DaprClient daprClient, ILogger<InventoryGateway> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<InventoryDto>> GetAvailabilityInventoryListAsync(
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("{Prefix}: GetAvailabilityInventoryListAsync", nameof(InventoryGateway));

            var httpExtension = new HTTPExtension {Verb = HTTPVerb.Post, ContentType = "application/json"};
            var data = new InventoryByIdsRequest();
            var inventories = await _daprClient.InvokeMethodAsync<InventoryByIdsRequest, List<InventoryDto>>(
                "inventoryapp", "get-inventories-by-ids",
                data, httpExtension, cancellationToken);

            return inventories;
        }
    }
}
