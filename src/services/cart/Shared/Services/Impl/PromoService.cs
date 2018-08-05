using VND.CoolStore.Services.Cart.Shared.Services;

namespace VND.CoolStore.Services.Cart.UseCases.v1.Services.Impl
{
  public class PromoService : IPromoService
  {
    public Domain.Cart ApplyCartItemPromotions(Domain.Cart cart)
    {
      // TODO: will calculate it later
      return cart;
    }

    public Domain.Cart ApplyShippingPromotions(Domain.Cart cart)
    {
      // TODO: will calculate it later
      return cart;
    }
  }
}
