using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog.Data
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(Guid productId);
        Task<IReadOnlyList<Product>> GetProductsAsync(int currentPage, double highPrice);
    }
}
