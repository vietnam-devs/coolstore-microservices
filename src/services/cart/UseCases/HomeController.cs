using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.Cart.UseCases
{
  [Route("")]
  [ApiVersionNeutral]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class HomeController : Controller
  {
    private string _basePath = "/";
    public HomeController(IConfiguration config)
    {
      _basePath = config.GetBasePath();
    }

    [HttpGet]
    public IActionResult Index()
    {
      return Redirect($"~{_basePath}swagger");
    }
  }
}
