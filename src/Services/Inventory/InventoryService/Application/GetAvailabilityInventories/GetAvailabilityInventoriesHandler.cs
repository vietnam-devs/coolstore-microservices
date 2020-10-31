using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Application.Common;
using MediatR;
using InventoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Application.GetAvailabilityInventories
{
    public class GetAvailabilityInventoriesHandler : IRequestHandler<GetAvailabilityInventoriesQuery, IEnumerable<Common.InventoryDto>>
    {
        private readonly IDbContextFactory<MainDbContext> _dbContextFactory;

        public GetAvailabilityInventoriesHandler(IDbContextFactory<MainDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<IEnumerable<InventoryDto>> Handle(GetAvailabilityInventoriesQuery request,
            CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (!request.Ids.Any())
            {
                return new List<InventoryDto>();
            }

            await using var dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Inventories
                .AsNoTracking()
                .Where(x => request.Ids.Contains(x.Id))
                .Select(x =>
                    new InventoryDto(x.Id, x.Location, x.Description, x.Website))
                .ToListAsync(cancellationToken);
        }
    }
}
