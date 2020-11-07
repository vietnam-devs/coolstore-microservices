using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.App.Dtos;
using ProductCatalogService.Domain.Gateway;
using ProductCatalogService.Infrastructure.Data;

namespace ProductCatalogService.Application.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameHandler : IRequestHandler<GetProductsByPriceAndNameQuery, IEnumerable<FlatProductDto>>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly IInventoryGateway _inventoryGateway;

        public GetProductsByPriceAndNameHandler(IDbContextFactory<MainDbContext> dbContextFactory,
            IInventoryGateway inventoryGateway)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _inventoryGateway = inventoryGateway ?? throw new ArgumentNullException(nameof(inventoryGateway));
        }

        public async Task<IEnumerable<FlatProductDto>> Handle(GetProductsByPriceAndNameQuery request,
            CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var products = await dbContext.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Skip(request.Page - 1)
                .Take(10) //TODO:
                .Where(x => !x.IsDeleted && x.Price <= request.Price)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            var inventoryIds = products.Select(x => x.InventoryId).Distinct();
            var inventories = await _inventoryGateway.GetInventoryListAsync(inventoryIds, cancellationToken);

            return products.Select(x =>
            {
                InventoryDto? inventory = null;
                if (inventories.Any())
                {
                    inventory = inventories.First(y => y.Id == x.InventoryId);
                }

                var product = new FlatProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    Description = x.Description,
                    CategoryId = x.Category.Id,
                    CategoryName = x.Category.Name
                };

                if (inventory is null) return product;

                product.InventoryId = inventory.Id;
                product.InventoryLocation = inventory.Location;
                product.InventoryWebsite = inventory.Website;
                product.InventoryDescription = inventory.Description;

                return product;
            });
        }
    }
}
