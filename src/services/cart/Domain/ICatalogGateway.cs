using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.v1.Grpc;

namespace VND.CoolStore.Services.Cart.Domain
{
    public interface ICatalogGateway
    {
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
