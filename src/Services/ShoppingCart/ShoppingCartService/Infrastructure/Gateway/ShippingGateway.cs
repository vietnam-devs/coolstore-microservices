using N8T.Infrastructure.App.Dtos;
using ShoppingCartService.Domain.Gateway;

namespace ShoppingCartService.Infrastructure.Gateway
{
    public class ShippingGateway : IShippingGateway
    {
        public CartDto CalculateShipping(CartDto cart)
        {
            return cart;
        }
    }
}
