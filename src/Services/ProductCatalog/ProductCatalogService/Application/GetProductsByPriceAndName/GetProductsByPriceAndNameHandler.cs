using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Application.Common;
using ProductCatalogService.Infrastructure.Data;

namespace ProductCatalogService.Application.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameHandler : IRequestHandler<GetProductsByPriceAndNameQuery, IEnumerable<ProductDto>>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly DaprClient _daprClient;

        public GetProductsByPriceAndNameHandler(IDbContextFactory<MainDbContext> dbContextFactory, DaprClient daprClient)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsByPriceAndNameQuery request,
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

                return new ProductDto(x.Id, x.Name, x.Price, x.ImageUrl, x.Description,
                    inventory?.Id, inventory?.Location, inventory?.Website, inventory?.Description,
                    x.Category.Id, x.Category.Name);
            });
        }
    }
}
