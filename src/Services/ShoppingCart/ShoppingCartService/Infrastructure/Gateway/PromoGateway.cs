using N8T.Infrastructure.App.Dtos;
using ShoppingCartService.Domain.Gateway;

namespace ShoppingCartService.Infrastructure.Gateway
{
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
}
