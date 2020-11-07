using System.Collections.Generic;
using InventoryService.Application.GetAvailabilityInventories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InventoryService.Application.GetInventory;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Requests.Inventory;

namespace InventoryService.Api.Http.Controllers
{
    [ApiController]
    public class InventoryDaprController : ControllerBase
    {
        [HttpPost("/get-inventories-by-ids")]
        public async Task<IEnumerable<InventoryDto>> GetByIds(InventoryByIdsRequest request,
            [FromServices] IMediator mediator)
        {
            var query = new GetAvailabilityInventoriesQuery {Ids = request.InventoryIds};
            var result = await mediator?.Send(query)!;
            return result;
        }

        [HttpPost("/get-inventory-by-id")]
        public async Task<InventoryDto> GetByIds(InventoryRequest request,
            [FromServices] IMediator mediator)
        {
            var query = new GetInventoryQuery {Id = request.InventoryId};
            var result = await mediator?.Send(query)!;
            return result;
        }
    }
}
