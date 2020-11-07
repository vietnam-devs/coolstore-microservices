using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.App.Dtos;
using ProductCatalogService.Domain.Gateway;
using ProductCatalogService.Domain.Model;
using ProductCatalogService.Infrastructure.Data;

namespace ProductCatalogService.Application.GetProductsByIds
{
    public class GetProductsByIdsHandler : IRequestHandler<GetProductsByIdsQuery, IEnumerable<ProductDto>>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly IInventoryGateway _inventoryGateway;

        public GetProductsByIdsHandler(IDbContextFactory<MainDbContext> dbContextFactory,
            IInventoryGateway inventoryGateway)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _inventoryGateway = inventoryGateway ?? throw new ArgumentNullException(nameof(inventoryGateway));
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            await using var dbContext = _dbContextFactory.CreateDbContext();

            if (!request.ProductIds.Any())
            {
                // get all products
                var products = await dbContext.Products
                    .Include(x => x.Category)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return await GetProducts(products, cancellationToken);
            }
            else
            {
                // get by product ids
                var products = await dbContext.Products
                    .Include(x => x.Category)
                    .AsNoTracking()
                    .Where(x => request.ProductIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                return await GetProducts(products, cancellationToken);
            }
        }

        private async Task<IEnumerable<ProductDto>> GetProducts(IEnumerable<Product> products,
            CancellationToken cancellationToken)
        {
            var inventories = await _inventoryGateway.GetInventoryListAsync(cancellationToken: cancellationToken);

            return products.Select(x =>
            {
                var product = new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    Description = x.Description,
                    Category = new CategoryDto {Id = x.Category.Id, Name = x.Category.Name}
                };

                var inv = inventories.FirstOrDefault(y => y.Id == x.InventoryId);

                if (inv is not null)
                {
                    product.Inventory = new InventoryDto
                    {
                        Id = inv.Id, Location = inv.Location, Website = inv.Website, Description = inv.Description
                    };
                }

                return product;
            });
        }
    }
}
