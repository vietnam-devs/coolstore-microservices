using ShoppingCart.Core.Dtos;
using ShoppingCart.Core.Gateways;

namespace ShoppingCart.Infrastructure.Gateways;

public class ShippingGateway : IShippingGateway
{
    public CartDto CalculateShipping(CartDto cart)
    {
        return cart;
    }
}
