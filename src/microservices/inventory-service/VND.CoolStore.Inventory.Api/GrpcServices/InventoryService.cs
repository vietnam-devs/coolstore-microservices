using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VND.CoolStore.Inventory.DataContracts.Api.V1;

namespace VND.CoolStore.Inventory.Api.GrpcServices
{
    [Authorize]
    public class InventoryService : InventoryApi.InventoryApiBase
    {
        private readonly IMediator _mediator;

        public InventoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetInventoriesResponse> GetInventories(GetInventoriesRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<GetInventoryResponse> GetInventory(GetInventoryRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }
    }
}
