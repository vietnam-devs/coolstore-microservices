using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartService.Domain.Dto;

namespace ShoppingCartService.Domain.Service
{
    public interface IProductCatalogService
    {
        FlatProductDto? GetProductById(Guid id);
        Task<FlatProductDto?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<FlatProductDto>> GetProductsAsync();
    }
}
