namespace VND.CoolStore.Services.Cart.Domain
{
    public interface IShippingGateway
    {
        Cart CalculateShipping(Cart cart);
    }
}
