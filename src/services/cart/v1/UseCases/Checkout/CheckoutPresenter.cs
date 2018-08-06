using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public sealed class CheckoutPresenter
  {
    public IActionResult Populate(CheckoutResponse output)
    {
      if (output == null)
      {
        return new NoContentResult();
      }

      return new OkObjectResult(output.IsSucceed);
    }
  }
}
