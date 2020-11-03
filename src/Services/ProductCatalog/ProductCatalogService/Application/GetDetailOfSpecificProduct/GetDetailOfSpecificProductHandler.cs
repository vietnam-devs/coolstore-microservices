using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Http;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.App.Dtos;
using ProductCatalogService.Domain.Exception;
using ProductCatalogService.Infrastructure.Data;

namespace ProductCatalogService.Application.GetDetailOfSpecificProduct
{
    public class GetDetailOfSpecificProductHandler : IRequestHandler<GetDetailOfSpecificProductQuery, FlatProductDto>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly DaprClient _daprClient;

        public GetDetailOfSpecificProductHandler(IDbContextFactory<MainDbContext> dbContextFactory,
            DaprClient daprClient)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        public async Task<FlatProductDto> Handle(GetDetailOfSpecificProductQuery request,
            CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var product = await dbContext.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product is null)
            {
                throw new NotFoundProductException(request.Id);
            }

            var httpExtension = new HTTPExtension {Verb = HTTPVerb.Post, ContentType = "application/json"};
            var requestData = new InventoryRequest(product.InventoryId);
            var inventory = await _daprClient.InvokeMethodAsync<InventoryRequest, InventoryDto>(
                "inventoryapp", "get-inventory-by-id", requestData, httpExtension, cancellationToken);

            if (inventory is null)
            {
                throw new NotFoundInventoryException(product.InventoryId);
            }

            return new FlatProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                InventoryId = inventory.Id,
                InventoryLocation = inventory.Location,
                InventoryWebsite = inventory.Website,
                InventoryDescription = inventory.Description,
                CategoryId = product.Category.Id,
                CategoryName = product.Category.Name
            };
        }
    }
}
