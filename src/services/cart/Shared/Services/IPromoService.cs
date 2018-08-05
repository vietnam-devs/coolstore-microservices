namespace VND.CoolStore.Services.Cart.Shared.Services
{
  public interface IPromoService
  {
    Domain.Cart ApplyCartItemPromotions(Domain.Cart cart);
    Domain.Cart ApplyShippingPromotions(Domain.Cart cart);
  }
}
