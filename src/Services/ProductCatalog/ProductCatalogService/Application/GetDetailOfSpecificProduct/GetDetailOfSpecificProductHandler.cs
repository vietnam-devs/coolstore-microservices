using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.App.Dtos;
using ProductCatalogService.Domain.Exception;
using ProductCatalogService.Domain.Gateway;
using ProductCatalogService.Infrastructure.Data;

namespace ProductCatalogService.Application.GetDetailOfSpecificProduct
{
    public class GetDetailOfSpecificProductHandler : IRequestHandler<GetDetailOfSpecificProductQuery, FlatProductDto>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
        private readonly IInventoryGateway _inventoryGateway;

        public GetDetailOfSpecificProductHandler(IDbContextFactory<MainDbContext> dbContextFactory,
            IInventoryGateway inventoryGateway)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _inventoryGateway = inventoryGateway ?? throw new ArgumentNullException(nameof(inventoryGateway));
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

            var inventory = await _inventoryGateway.GetInventoryByIdAsync(product.InventoryId, cancellationToken);

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
