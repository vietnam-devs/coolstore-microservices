using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Domain.Gateway
{
    public interface IShippingGateway
    {
        CartDto CalculateShipping(CartDto cart);
    }
}
