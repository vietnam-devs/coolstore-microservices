using ShoppingCart.Core.Dtos;

namespace ShoppingCart.Core.Gateways;

public interface IPromoGateway
{
    CartDto ApplyCartItemPromotions(CartDto cart);
    CartDto ApplyShippingPromotions(CartDto cart);
}
