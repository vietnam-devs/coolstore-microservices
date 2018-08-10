using System;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Review.Infrastructure.Db;
using VND.Fw.Infrastructure.EfCore.Extensions;

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
