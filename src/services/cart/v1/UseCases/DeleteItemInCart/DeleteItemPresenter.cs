using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public sealed class DeleteItemPresenter
  {
    public IActionResult Populate(DeleteItemResponse output)
    {
      if (output == null)
      {
        return new NoContentResult();
      }

      return new OkObjectResult(output.ProductId);
    }
  }
}
