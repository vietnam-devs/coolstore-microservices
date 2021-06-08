using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Requests.ProductCatalog;
using SaleService.Domain.Gateway;

namespace SaleService.Infrastructure.Gateway
{
    public class ProductCatalogGateway : IProductCatalogGateway
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ProductCatalogGateway> _logger;

        public ProductCatalogGateway(DaprClient daprClient, ILogger<ProductCatalogGateway> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ProductDto>> GetProductByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("{Prefix}: GetProductByIdsAsync by id={Id}", nameof(ProductCatalogGateway),
                ids.Aggregate("", (x, y) => $"{x},{y}"));

            var requestData = new ProductByIdsRequest {ProductIds = ids.ToList()};
            var products = await _daprClient.InvokeMethodAsync<ProductByIdsRequest, List<ProductDto>>(
                "productcatalogapp", "get-products-by-ids", requestData, cancellationToken);

            if (products is null)
            {
                throw new Exception("Products might be null.");
            }

            return products;
        }
    }
}
