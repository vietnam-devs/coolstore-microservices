using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Service;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/inventories")]
  public class InventoryController : FW.Infrastructure.AspNetCore.ControllerBase
  {
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
      _inventoryService = inventoryService;
    }

    [HttpGet]
    [Auth(Policy = "access_inventory_api")]
    [SwaggerOperation(Tags = new[] { "inventory-service" })]
    [Route("availability/{itemId:guid}")]
    public async Task<ActionResult<InventoryModel>> Availability(Guid itemId)
    {
      var result = await _inventoryService.GetAvailabilityAsync(itemId);
      return Ok(result);
    }
  }
}
