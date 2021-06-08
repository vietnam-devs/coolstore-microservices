using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SaleService.Domain.Gateway;
using SaleService.Domain.Model;

namespace SaleService.Infrastructure.Services
{
    public class OrderValidationService : IOrderValidationService
    {
        private readonly IInventoryGateway _inventoryGateway;
        private readonly IProductCatalogGateway _productCatalogGateway;
        private readonly ILogger<OrderValidationService> _logger;

        public OrderValidationService(IInventoryGateway inventoryGateway, IProductCatalogGateway productCatalogGateway, ILogger<OrderValidationService> logger)
        {
            _inventoryGateway = inventoryGateway ?? throw new ArgumentNullException(nameof(inventoryGateway));
            _productCatalogGateway = productCatalogGateway ?? throw new ArgumentNullException(nameof(productCatalogGateway));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> ValidateInventoriesAsync(IEnumerable<Guid> inventoryIds, CancellationToken cancellationToken = default)
        {
            if (!inventoryIds.Any()) return true;

            var inventories = await _inventoryGateway.GetInventoryListAsync(inventoryIds, cancellationToken);
            return inventoryIds.Count() == inventories.Count() && !inventories.Any(x => Guid.Empty == x.Id);
        }

        public async Task<bool> ValidateProductsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default)
        {
            if (!productIds.Any()) return true;

            var products = await _productCatalogGateway.GetProductByIdsAsync(productIds, cancellationToken);
            return productIds.Count() == products.Count() && !products.Any(x => Guid.Empty == x.Id);
        }
    }
}
