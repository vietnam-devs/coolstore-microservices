using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Domain.Gateway
{
    public interface IInventoryGateway
    {
        Task<IEnumerable<InventoryDto>> GetAvailabilityInventoryListAsync(CancellationToken cancellationToken = default);
    }
}
