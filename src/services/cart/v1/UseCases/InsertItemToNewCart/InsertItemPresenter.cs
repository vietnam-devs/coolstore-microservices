using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  public sealed class InsertItemPresenter
  {
    public IActionResult Populate(InsertItemToNewCartResponse output)
    {
      if (output == null)
      {
        return new NoContentResult();
      }

      return new OkObjectResult(output?.Result);
    }
  }
}
