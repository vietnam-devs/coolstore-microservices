using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SaleService.Domain.Model
{
    public interface IOrderValidationService
    {
        Task<bool> ValidateInventoriesAsync(IEnumerable<Guid> inventoryIds, CancellationToken cancellationToken = default);
        Task<bool> ValidateProductsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default);
    }
}
