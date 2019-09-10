namespace VND.CoolStore.ShoppingCart.Domain
{
    public interface IPromoGateway
    {
        Cart ApplyCartItemPromotions(Cart cart);
        Cart ApplyShippingPromotions(Cart cart);
    }
}
