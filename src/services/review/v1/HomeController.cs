using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NetCoreKit.Infrastructure.AspNetCore.Extensions;
using NetCoreKit.Infrastructure.AspNetCore.Miniservice.Options;

namespace VND.CoolStore.Services.Review.v1
{
  [Route("")]
  [ApiVersionNeutral]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class HomeController : Controller
  {
    private readonly string _basePath;
    public HomeController(IConfiguration config, IOptions<PersistenceOption> options)
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
