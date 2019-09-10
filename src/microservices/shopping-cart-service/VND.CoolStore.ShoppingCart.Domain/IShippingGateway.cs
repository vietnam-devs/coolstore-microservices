namespace VND.CoolStore.ShoppingCart.Domain
{
    public interface IShippingGateway
    {
        Cart CalculateShipping(Cart cart);
    }
}
