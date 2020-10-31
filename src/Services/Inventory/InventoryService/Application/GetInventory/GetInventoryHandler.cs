using System;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Application.Common;
using InventoryService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            return new InventoryDto(inv.Id, inv.Location, inv.Description, inv.Website);
        }
    }
}
