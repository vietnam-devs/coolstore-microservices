using ShoppingCartService.Domain.Model;

namespace ShoppingCartService.Domain.Gateway
{
    public interface IShippingGateway
    {
        ShoppingCart CalculateShipping(ShoppingCart cart);
    }
}
