using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coolstore;
using VND.CoolStore.Services.Cart.Domain;
using static Coolstore.CatalogService;

namespace VND.CoolStore.Services.Cart.Infrastructure.Gateways
{
    public class CatalogGateway : ICatalogGateway
    {
        private readonly CatalogServiceClient _catalogServiceClient;
        public CatalogGateway(CatalogServiceClient catalogServiceClient)
        {
            _catalogServiceClient = catalogServiceClient;
        }

        public async Task<CatalogProductDto> GetProductByIdAsync(Guid id)
        {
            //var getProductEndPoint = $"{_catalogServiceUri}/api/products/{id}";
            //var response = await RestClient.GetAsync<ProductDto>(getProductEndPoint);
            //return response;

            var response = await _catalogServiceClient.GetProductByIdAsync(new GetProductByIdRequest {
                ProductId = id.ToString()
            });

            return response.Product;
        }

        public async Task<IEnumerable<CatalogProductDto>> GetProductsAsync()
        {
            //var getProductsEndPoint = $"{_catalogServiceUri}/api/products";
            //var responses = await RestClient.GetAsync<List<ProductDto>>(getProductsEndPoint);
            //return responses;

            var response = await _catalogServiceClient.GetProductsAsync(new GetProductsRequest{});
            return response.Products;
        }
    }
}
