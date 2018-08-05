using Microsoft.AspNetCore.Mvc;
using System;
using VND.CoolStore.Services.Inventory.UseCases.Service;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.Inventory.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/availability")]
  public class AvailabilityController : Controller
  {
    private readonly IInventoryService _inventoryService = null;

    public AvailabilityController(IInventoryService inventoryService)
    {
      _inventoryService = inventoryService;
    }

    [HttpGet("{id}")]
    [Auth(Policy = "access_inventory_api")]
    public ActionResult<Domain.Inventory> Get(Guid id)
    {
      return _inventoryService.GetInventory(id);
    }

    [HttpGet]
    public ActionResult<string> Sample()
    {
      return "1.0";
    }
  }
}
