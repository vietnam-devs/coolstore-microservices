using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using N8T.Infrastructure.App.Dtos;

namespace SaleService.Domain.Gateway
{
    public interface IProductCatalogGateway
    {
        Task<IEnumerable<ProductDto>> GetProductByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    }
}
