using System;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Inventory.Infrastructure.Db;

namespace VND.CoolStore.Services.Inventory.v1.UseCases
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
        _serviceProvider.MigrateDbContext<InventoryDbContext>();
      }
      catch (Exception)
      {

        return new BadRequestResult();
      }
      
      return Ok();
    }
  }
}
