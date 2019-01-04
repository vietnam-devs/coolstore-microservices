using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore.Authz;
using MyInventoryService = VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;

namespace VND.CoolStore.Services.WebAggregator.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/migrates")]
    public class DbMigrationController : ControllerBase
    {
        private readonly MyInventoryService.InventoryServiceClient _inventoryServiceClient;

        public DbMigrationController(MyInventoryService.InventoryServiceClient inventoryServiceClient)
        {
            _inventoryServiceClient = inventoryServiceClient;
        }

        [HttpPost("migrate")]
        //[Auth(Policy = "access_inventory_api")]
        public async Task<IActionResult> DbMigration()
        {
            var result = await _inventoryServiceClient.DbMigrationAsync(new Empty());
            return Ok(result);
        }
    }
}
