namespace VND.CoolStore.ShoppingCart.Domain.Cart
{
    public interface IPromoGateway
    {
        Cart ApplyCartItemPromotions(Cart cart);
        Cart ApplyShippingPromotions(Cart cart);
    }
}
