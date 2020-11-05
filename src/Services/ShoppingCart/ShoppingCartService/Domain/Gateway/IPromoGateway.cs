using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Domain.Gateway
{
    public interface IPromoGateway
    {
        CartDto ApplyCartItemPromotions(CartDto cart);
        CartDto ApplyShippingPromotions(CartDto cart);
    }
}
