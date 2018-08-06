namespace VND.CoolStore.Services.Cart.Infrastructure.Gateways
{
  public interface IPromoGateway
  {
    Domain.Cart ApplyCartItemPromotions(Domain.Cart cart);
    Domain.Cart ApplyShippingPromotions(Domain.Cart cart);
  }
}
