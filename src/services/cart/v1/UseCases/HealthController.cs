using System;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Cart.Infrastructure.Db;
using VND.Fw.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases
{
  [Route("")]
  [ApiVersionNeutral]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class HealthController : Controller
  {
    private IServiceProvider _serviceProvider;
    public HealthController(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    [HttpGet("/healthz")]
    public ActionResult Get()
    {
      try
      {
        _serviceProvider.MigrateDbContext<CartDbContext>();
      }
      catch (Exception)
      {

        return new BadRequestResult();
      }

      return Ok();
    }
  }
}
