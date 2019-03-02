using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Grpc;

namespace VND.CoolStore.Services.Cart.Infrastructure.Gateways
{
    public class CatalogGateway : ICatalogGateway
    {
        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            //var getProductEndPoint = $"{_catalogServiceUri}/api/products/{id}";
            //var response = await RestClient.GetAsync<ProductDto>(getProductEndPoint);
            //return response;

            return await Task.FromResult(new ProductDto());
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            //var getProductsEndPoint = $"{_catalogServiceUri}/api/products";
            //var responses = await RestClient.GetAsync<List<ProductDto>>(getProductsEndPoint);
            //return responses;

            return await Task.FromResult(new List<ProductDto>());
        }
    }
}
