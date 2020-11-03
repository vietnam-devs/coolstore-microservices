using ShoppingCartService.Domain.Gateway;
using ShoppingCartService.Domain.Model;

namespace ShoppingCartService.Infrastructure.Gateway
{
    public class PromoGateway : IPromoGateway
    {
        public ShoppingCart ApplyCartItemPromotions(ShoppingCart cart)
        {
            return cart;
        }

        public ShoppingCart ApplyShippingPromotions(ShoppingCart cart)
        {
            return cart;
        }
    }
}
