using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.App.Dtos;
using ProductCatalogService.Infrastructure.Data;

namespace ProductCatalogService.Application.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameHandler : IRequestHandler<GetProductsByPriceAndNameQuery, IEnumerable<FlatProductDto>>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly DaprClient _daprClient;

        public GetProductsByPriceAndNameHandler(IDbContextFactory<MainDbContext> dbContextFactory, DaprClient daprClient)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        public async Task<IEnumerable<FlatProductDto>> Handle(GetProductsByPriceAndNameQuery request,
            CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var products = await dbContext.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Skip(request.Page - 1)
                .Take(10)
                .Where(x => !x.IsDeleted && x.Price <= request.Price)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            var categoryIds = products.Select(x => x.InventoryId).Distinct();

            var httpExtension = new HTTPExtension {Verb = HTTPVerb.Post, ContentType = "application/json"};
            var data = new GetInventoryByIdsRequest(categoryIds.ToList());
            var inventories = await _daprClient.InvokeMethodAsync<GetInventoryByIdsRequest, List<InventoryDto>>(
                "inventoryapp", "get-inventories-by-ids",
                data, httpExtension, cancellationToken);

            return products.Select(x =>
            {
                InventoryDto? inventory = null;
                if (inventories is not null && inventories.Count > 0)
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
