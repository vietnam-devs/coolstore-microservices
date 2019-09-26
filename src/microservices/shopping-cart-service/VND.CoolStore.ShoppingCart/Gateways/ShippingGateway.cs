using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.Gateways
{
    public class ShippingGateway : IShippingGateway
    {
        public Cart CalculateShipping(Cart cart)
        {
            // TODO: will calculate it later
            return cart;
        }
    }
}
