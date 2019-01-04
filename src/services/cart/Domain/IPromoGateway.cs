namespace VND.CoolStore.Services.Cart.Domain
{
    public interface IPromoGateway
    {
        Cart ApplyCartItemPromotions(Cart cart);
        Cart ApplyShippingPromotions(Cart cart);
    }
}
