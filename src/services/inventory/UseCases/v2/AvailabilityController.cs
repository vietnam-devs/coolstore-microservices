using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.Services.Inventory.UseCases.v2
{
  [ApiVersion("2.0")]
  [Route("api/availability")]
  public class AvailabilityController : Controller
  {
    [HttpGet]
    public ActionResult<string> Sample()
    {
      return "2.0";
    }
  }
}
