using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore.Authz;
using VND.CoolStore.Services.Inventory.v1.Grpc;
using MyInventoryService = VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;

namespace VND.CoolStore.Services.WebAggregator.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/availabilities")]
    public class InventoryController : ControllerBase
    {
        private readonly MyInventoryService.InventoryServiceClient _inventoryServiceClient;

        public InventoryController(MyInventoryService.InventoryServiceClient inventoryServiceClient)
        {
            _inventoryServiceClient = inventoryServiceClient;
        }

        [HttpGet("")]
        //[Auth(Policy = "access_inventory_api")]
        public async Task<IActionResult> Get()
        {
            var result = await _inventoryServiceClient.GetInventoriesAsync(new Empty());
            return Ok(result);
        }

        [HttpGet("{id}")]
        //[Auth(Policy = "access_inventory_api")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _inventoryServiceClient.GetInventoryAsync(new GetInventoryRequest { Id = id.ToString() });
            return Ok(result);
        }
    }
}
