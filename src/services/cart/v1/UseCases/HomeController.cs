using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.AspNetCore.Miniservice.Options;

namespace VND.CoolStore.Services.Cart.v1.UseCases
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
