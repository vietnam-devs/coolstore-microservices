using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore.Authz;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using VND.CoolStore.Services.Inventory.v1.UseCases.GetInventory;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace VND.CoolStore.Services.Inventory.v1.UseCases
{
  [ApiVersion("1.0")]
  [Route("api/availability")]
  public class AvailabilityController : Controller
  {
    [HttpGet("{id}")]
    [Auth(Policy = "access_inventory_api")]
    public async Task<IActionResult> Get([FromServices] IMediator eventor, Guid id, CancellationToken cancellationToken)
    {
      return await eventor.SendStream<GetInventoryRequest, GetInventoryResponse>(
        new GetInventoryRequest { InventoryId = id },
        x => x.Result);
    }

    [HttpGet]
    public ActionResult<string> Sample(CancellationToken cancellationToken)
    {
      return "1.0";
    }
  }
}
