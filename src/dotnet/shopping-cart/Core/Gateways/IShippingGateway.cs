using ShoppingCart.Core.Dtos;

namespace ShoppingCart.Core.Gateways;

public interface IShippingGateway
{
    CartDto CalculateShipping(CartDto cart);
}
