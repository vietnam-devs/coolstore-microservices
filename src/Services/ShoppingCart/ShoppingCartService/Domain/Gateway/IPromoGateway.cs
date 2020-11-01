using ShoppingCartService.Domain.Model;

namespace ShoppingCartService.Domain.Gateway
{
    public interface IPromoGateway
    {
        ShoppingCart ApplyCartItemPromotions(ShoppingCart cart);
        ShoppingCart ApplyShippingPromotions(ShoppingCart cart);
    }
}
