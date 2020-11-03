using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using InventoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using N8T.Infrastructure.App.Dtos;

namespace InventoryService.Application.GetAvailabilityInventories
{
    public class GetAvailabilityInventoriesHandler : IRequestHandler<GetAvailabilityInventoriesQuery, IEnumerable<InventoryDto>>
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

            await using var dbContext = _dbContextFactory.CreateDbContext();

            if (!request.Ids.Any())
            {
                // get all inventories
                return await dbContext.Inventories
                    .AsNoTracking()
                    .Select(x =>
                        new InventoryDto
                        {
                            Id = x.Id, Location = x.Location, Description = x.Description, Website = x.Website
                        })
                    .ToListAsync(cancellationToken);
            }

            return await dbContext.Inventories
                .AsNoTracking()
                .Where(x => request.Ids.Contains(x.Id))
                .Select(x =>
                    new InventoryDto
                    {
                        Id = x.Id, Location = x.Location, Description = x.Description, Website = x.Website
                    })
                .ToListAsync(cancellationToken);
        }
    }
}
