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

            var inventoryTags = new Dictionary<Guid, int>();
            var categoryTags = new Dictionary<Guid, int>();

            var products = await dbContext.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Skip(request.Page - 1)
                .Take(request.PageSize)
                .Where(x => !x.IsDeleted && x.Price <= request.Price)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            _ = products
                .Select(x =>
                {
                    inventoryTags.TryAdd(x.InventoryId, 0);
                    return x;
                })
                .ToList();

            _ = products
                .Select(x =>
                {
                    categoryTags.TryAdd(x.CategoryId, 0);
                    return x;
                })
                .ToList();

            var total = await dbContext.Products.AsNoTracking()
                .CountAsync(x => !x.IsDeleted, cancellationToken: cancellationToken);

            var inventories = await _inventoryGateway.GetInventoryListAsync(cancellationToken: cancellationToken);
            var inventoryWithIndex = ReverseInventory(inventories);

            var categories = products.Select(x => new CategoryDto {Id = x.Category.Id, Name = x.Category.Name});
            var categoryWithIndex = ReverseCategory(categories);

            var items = products.Select(x =>
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

                InventoryDto? inventory = default;
                if (inventories.Any())
                {
                    inventory = inventories.FirstOrDefault(y => y.Id == x.InventoryId);
                }

                if (inventory is default(InventoryDto)) return product;

                categoryTags[x.CategoryId] = categoryTags[x.CategoryId] + 1;
                inventoryTags[inventory.Id] = inventoryTags[inventory.Id] + 1;

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
                items.ToList(),
                new List<SearchAggsByTagsDto>(inventoryTags.Select(x =>
                    new SearchAggsByTagsDto(inventoryWithIndex[x.Key].Location, x.Value))),
                new List<SearchAggsByTagsDto>(categoryTags.Select(x =>
                    new SearchAggsByTagsDto(categoryWithIndex[x.Key].Name, x.Value))),
                0);

            return result;
        }

        private static IDictionary<Guid, InventoryDto> ReverseInventory(IEnumerable<InventoryDto> inventories)
        {
            var output = new Dictionary<Guid, InventoryDto>();
            _ = inventories.Select(x =>
                {
                    output.TryAdd(x.Id, x);
                    return x;
                })
                .ToList();

            return output;
        }

        private static IDictionary<Guid, CategoryDto> ReverseCategory(IEnumerable<CategoryDto> categories)
        {
            var output = new Dictionary<Guid, CategoryDto>();
            _ = categories.Select(x =>
                {
                    output.TryAdd(x.Id, x);
                    return x;
                })
                .ToList();

            return output;
        }
    }
}
