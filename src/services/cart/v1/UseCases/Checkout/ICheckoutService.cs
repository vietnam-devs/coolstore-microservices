using System.Threading.Tasks;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public interface ICheckoutService
  {
    Task<CheckoutResponse> Execute(CheckoutRequest request);
  }
}
