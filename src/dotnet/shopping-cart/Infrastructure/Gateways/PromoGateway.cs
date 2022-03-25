using ShoppingCart.Core.Dtos;
using ShoppingCart.Core.Gateways;

namespace ShoppingCart.Infrastructure.Gateways;

public class PromoGateway : IPromoGateway
{
    public CartDto ApplyCartItemPromotions(CartDto cart)
    {
        return cart;
    }

    public CartDto ApplyShippingPromotions(CartDto cart)
    {
        return cart;
    }
}
