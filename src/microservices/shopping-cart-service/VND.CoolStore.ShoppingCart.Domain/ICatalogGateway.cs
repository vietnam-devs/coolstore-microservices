using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.Domain
{
    public interface ICatalogGateway
    {
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
