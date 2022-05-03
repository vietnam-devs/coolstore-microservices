using ShoppingCart.Core.Dtos;

namespace ShoppingCart.Core.Gateways;

public interface IProductCatalogGateway
{
    Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
