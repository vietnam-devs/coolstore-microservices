using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using N8T.Domain;
using N8T.Infrastructure.App.Dtos;
using ShoppingCartService.Domain.Service;

namespace ShoppingCartService.Infrastructure.Service
{
    public class ProductCatalogService : IProductCatalogService
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ProductCatalogService> _logger;

        public ProductCatalogService(DaprClient daprClient, ILogger<ProductCatalogService> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // public ProductDto? GetProductById(Guid id)
        // {
        //     return new ProductDto(); //TODO
        // }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            _logger.LogInformation("{Prefix}: GetProductByIdAsync by id={Id}", nameof(ProductCatalogService), id);

            var products = await _daprClient.GetStateAsync<List<ProductDto>>("statestore", "products");
            if (products is null || products.Count <= 0)
            {
                throw new CoreException("Couldn't not find any product.");
            }

            return products.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            _logger.LogInformation("{Prefix}: GetProductsAsync", nameof(ProductCatalogService));

            var products = await _daprClient.GetStateAsync<List<ProductDto>>("statestore", "products");
            if (products is null || products.Count <= 0)
            {
                throw new CoreException("Couldn't not find any product.");
            }

            return products;
        }
    }
}
