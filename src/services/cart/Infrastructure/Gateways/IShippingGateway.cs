namespace VND.CoolStore.Services.Cart.Infrastructure.Gateways
{
  public interface IShippingGateway
  {
    Domain.Cart CalculateShipping(Domain.Cart cart);
  }
}
