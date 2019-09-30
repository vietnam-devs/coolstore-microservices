using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.ShoppingCart.DataContracts.Dto.V1;

namespace VND.CoolStore.ShoppingCart.Domain
{
    public interface IProductCatalogService
    {
        ProductDto GetProductById(Guid id);
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
