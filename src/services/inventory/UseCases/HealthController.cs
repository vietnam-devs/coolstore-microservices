using System;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Inventory.Infrastructure.Db;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Inventory.UseCases
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
