using System;
using Microsoft.AspNetCore.Mvc;
using VND.Services.Inventory.v1.Service;

namespace VND.Services.Inventory.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    public class AvailabilityController : Controller
    {
        private readonly IInventoryService _inventoryService = null;

        public AvailabilityController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{id}")]
        public Entity.Inventory Get(Guid id)
        {
            return _inventoryService.GetInventory(id);
        }
    }
}
