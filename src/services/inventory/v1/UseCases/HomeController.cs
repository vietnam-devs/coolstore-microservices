using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VND.Fw.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.Inventory.v1.UseCases
{
  [Route("")]
  [ApiVersionNeutral]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class HomeController : Controller
  {
    private readonly string _basePath;
    public HomeController(IConfiguration config)
    {
      _basePath = config.GetBasePath() ?? "/";
    }

    [HttpGet]
    public IActionResult Index()
    {
      return Redirect($"~{_basePath}swagger");
    }
  }
}
