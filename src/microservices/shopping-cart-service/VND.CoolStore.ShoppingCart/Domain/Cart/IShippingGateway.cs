namespace VND.CoolStore.ShoppingCart.Domain.Cart
{
    public interface IShippingGateway
    {
        Cart CalculateShipping(Cart cart);
    }
}
