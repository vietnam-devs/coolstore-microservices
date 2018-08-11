using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Inventory.v1.UseCases.GetInventory
{
  [ApiVersion("1.0")]
  [Route("api/availability")]
  public class Controller : EvtControllerBase
  {
    public Controller(IMediator mediator) : base(mediator) {}

    [HttpGet("{id}")]
    [Auth(Policy = "access_inventory_api")]
    public async Task<IActionResult> Get(Guid id)
    {
      return await Eventor.SendStream<GetInventoryRequest, GetInventoryResponse>(
        new GetInventoryRequest { InventoryId = id },
        x => x.Result);
    }

    [HttpGet]
    public ActionResult<string> Sample()
    {
      return "1.0";
    }
  }
}
