using System;
using System.Threading;
using System.Threading.Tasks;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Domain.Gateway
{
    public interface IProductCatalogGateway
    {
        Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
