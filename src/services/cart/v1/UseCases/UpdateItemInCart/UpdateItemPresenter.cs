using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public sealed class UpdateItemPresenter
  {
    public IActionResult Populate(UpdateItemInCartResponse output)
    {
      if (output == null)
      {
        return new NoContentResult();
      }

      return new OkObjectResult(output?.Result);
    }
  }
}
