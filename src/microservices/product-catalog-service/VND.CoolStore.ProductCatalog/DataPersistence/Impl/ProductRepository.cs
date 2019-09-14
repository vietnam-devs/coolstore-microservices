using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog.DataPersistence.Impl
{
    public class ProductRepository : IProductRepository
    {
        public Task<Product> GetProductAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetProductsAsync(int currentPage, double highPrice)
        {
            throw new NotImplementedException();
        }
    }
}
