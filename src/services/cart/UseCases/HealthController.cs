using Microsoft.AspNetCore.Mvc;

namespace VND.CoolStore.Services.Cart.UseCases
{
  [Route("")]
  [ApiVersionNeutral]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class HealthController : Controller
  {
    [HttpGet("/healthz")]
    public bool Get()
    {
      return true;
    }
  }
}
