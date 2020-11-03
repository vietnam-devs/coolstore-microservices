using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Domain.Service
{
    public interface IProductCatalogService
    {
        //ProductDto? GetProductById(Guid id);
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsAsync();
    }
}
