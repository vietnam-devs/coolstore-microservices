using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  public sealed class GetCartPresenter
  {
    public IActionResult Populate(GetCartResponse output)
    {
      if (output == null)
      {
        return new NoContentResult();
      }

      return new OkObjectResult(output?.Result);
    }
  }
}
