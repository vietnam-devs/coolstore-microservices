namespace VND.CoolStore.Services.Cart.Infrastructure.Service
{
  public interface IPromoService
  {
    Domain.Cart ApplyCartItemPromotions(Domain.Cart cart);
    Domain.Cart ApplyShippingPromotions(Domain.Cart cart);
  }
}
