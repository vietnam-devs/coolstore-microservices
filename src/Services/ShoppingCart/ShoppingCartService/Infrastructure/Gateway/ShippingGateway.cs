using ShoppingCartService.Domain.Gateway;
using ShoppingCartService.Domain.Model;

namespace ShoppingCartService.Infrastructure.Gateway
{
    public class ShippingGateway : IShippingGateway
    {
        public ShoppingCart CalculateShipping(ShoppingCart cart)
        {
            return cart;
        }
    }
}
