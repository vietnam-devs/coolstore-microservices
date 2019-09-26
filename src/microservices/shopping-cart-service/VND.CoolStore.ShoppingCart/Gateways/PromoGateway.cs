using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.Gateways
{
    public class PromoGateway : IPromoGateway
    {
        public Cart ApplyCartItemPromotions(Cart cart)
        {
            // TODO: will calculate it later
            return cart;
        }

        public Cart ApplyShippingPromotions(Cart cart)
        {
            // TODO: will calculate it later
            return cart;
        }
    }
}
