using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Requests.Inventory;
using SaleService.Domain.Gateway;

namespace SaleService.Infrastructure.Gateway
{
    public class InventoryGateway : IInventoryGateway
    {
        private readonly DaprClient _daprClient;

        public InventoryGateway(DaprClient daprClient)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        public async Task<IEnumerable<InventoryDto>> GetInventoryListAsync(IEnumerable<Guid>? ids = null,
            CancellationToken cancellationToken = default)
        {
            ids ??= new List<Guid>();
            var data = new InventoryByIdsRequest {InventoryIds = ids};

            var inventories = await _daprClient.InvokeMethodAsync<InventoryByIdsRequest, List<InventoryDto>>(
                "inventoryapp", "get-inventories-by-ids",
                data, cancellationToken: cancellationToken);

            return inventories;
        }
    }
}
