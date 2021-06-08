using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using N8T.Infrastructure.App.Dtos;

namespace SaleService.Domain.Gateway
{
    public interface IInventoryGateway
    {
        Task<IEnumerable<InventoryDto>> GetInventoryListAsync(IEnumerable<Guid>? ids = null,
            CancellationToken cancellationToken = default);
    }
}
