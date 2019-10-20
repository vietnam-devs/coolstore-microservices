using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using Microsoft.EntityFrameworkCore;
using VND.CoolStore.ShoppingCart.DataContracts.Dto.V1;
using VND.CoolStore.ShoppingCart.Domain;

namespace VND.CoolStore.ShoppingCart.Data.Services
{
    public class ProductCatalogService : IProductCatalogService
    {
        private readonly IEfUnitOfWork<ShoppingCartDataContext> _unitOfWork;

        public ProductCatalogService(IEfUnitOfWork<ShoppingCartDataContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProductDto GetProductById(Guid id)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.ProductCatalog.ProductCatalog, Guid>();

            return queryRepository.Queryable()
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    Desc = p.Desc,
                    ImagePath = p.ImagePath,
                    InventoryId = p.InventoryId.ToString(),
                    IsDeleted = p.IsDeleted
                })
                .SingleOrDefault();
        }

        public Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.ProductCatalog.ProductCatalog, Guid>();

            var product = queryRepository.Queryable()
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    Desc = p.Desc,
                    ImagePath = p.ImagePath,
                    InventoryId = p.InventoryId.ToString(),
                    IsDeleted = p.IsDeleted
                })
                .SingleOrDefault();

            return Task.FromResult(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var queryRepository = _unitOfWork.QueryRepository<Domain.ProductCatalog.ProductCatalog, Guid>();

            return await queryRepository.Queryable()
                .Select(p => new ProductDto
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    Desc = p.Desc,
                    ImagePath = p.ImagePath,
                    InventoryId = p.InventoryId.ToString(),
                    IsDeleted = p.IsDeleted
                })
                .ToListAsync();
        }
    }
}
