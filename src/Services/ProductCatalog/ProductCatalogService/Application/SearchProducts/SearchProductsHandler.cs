using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.App.Dtos;
using ProductCatalogService.Application.Common;
using ProductCatalogService.Domain.Gateway;
using ProductCatalogService.Infrastructure.Data;

namespace ProductCatalogService.Application.SearchProducts
{
    public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, SearchProductsResponse>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly IInventoryGateway _inventoryGateway;

        public SearchProductsHandler(IDbContextFactory<MainDbContext> dbContextFactory,
            IInventoryGateway inventoryGateway)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _inventoryGateway = inventoryGateway ?? throw new ArgumentNullException(nameof(inventoryGateway));
        }

        public async Task<SearchProductsResponse> Handle(SearchProductsQuery request,
            CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var products = await dbContext.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Skip(request.Page - 1)
                .Take(request.PageSize)
                .Where(x => !x.IsDeleted && x.Price <= request.Price)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            var total = await dbContext.Products.AsNoTracking()
                .CountAsync(x => !x.IsDeleted, cancellationToken: cancellationToken);

            var inventories = await _inventoryGateway.GetInventoryListAsync(cancellationToken: cancellationToken);

            var items = products.Select(x =>
            {
                InventoryDto? inventory = null;
                if (inventories.Any())
                {
                    inventory = inventories.First(y => y.Id == x.InventoryId);
                }

                var product = new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl,
                    Description = x.Description,
                    Category =
                        new CategoryDto
                        {
                            Id = x.Category != null ? x.Category.Id : Guid.Empty,
                            Name = x.Category != null ? x.Category.Name : string.Empty
                        },
                };

                if (inventory is null) return product;

                product.Inventory = new InventoryDto
                {
                    Id = inventory.Id,
                    Location = inventory.Location,
                    Website = inventory.Website,
                    Description = inventory.Description
                };

                return product;
            });

            var result = new SearchProductsResponse(
                total,
                request.Page,
                items,
                new List<SearchAggsByTagsDto>(), //TODO
                new List<SearchAggsByTagsDto>(), //TODO
                0);

            return result;
        }
    }
}
