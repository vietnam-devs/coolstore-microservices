namespace VND.CoolStore.Services.Cart.Infrastructure.Service
{
  public interface IShippingService
  {
    Domain.Cart CalculateShipping(Domain.Cart cart);
  }
}
