namespace VND.CoolStore.Services.Cart.Shared.Services
{
  public interface IShippingService
  {
    Domain.Cart CalculateShipping(Domain.Cart cart);
  }
}
