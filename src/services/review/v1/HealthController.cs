using System;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Review.Infrastructure.Db;

namespace VND.CoolStore.Services.Review.v1
{
  [Route("")]
  [ApiVersionNeutral]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class HealthController : Controller
  {
    private readonly IServiceProvider _serviceProvider;
    public HealthController(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    [HttpGet("/healthz")]
    public ActionResult Get()
    {
      try
      {
        _serviceProvider.MigrateDbContext<ReviewDbContext>();
      }
      catch (Exception)
      {

        return new BadRequestResult();
      }

      return Ok();
    }
  }
}
