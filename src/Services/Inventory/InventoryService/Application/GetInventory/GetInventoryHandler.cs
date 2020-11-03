using System;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.App.Dtos;

namespace InventoryService.Application.GetInventory
{
    public class GetInventoryHandler : IRequestHandler<GetInventoryQuery, InventoryDto>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

        public GetInventoryHandler(IDbContextFactory<MainDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<InventoryDto> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var inv = await dbContext.Inventories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return new InventoryDto
            {
                Id = inv.Id, Location = inv.Location, Description = inv.Description, Website = inv.Website
            };
        }
    }
}
