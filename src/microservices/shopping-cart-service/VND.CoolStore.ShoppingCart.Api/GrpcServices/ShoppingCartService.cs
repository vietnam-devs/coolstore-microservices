using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using VND.CoolStore.ShoppingCart.DataContracts.Api.V1;

namespace VND.CoolStore.ShoppingCart.Api.GrpcServices
{
    [Authorize]
    public class ShoppingCartService : ShoppingCartApi.ShoppingCartApiBase
    {
        private readonly IMediator _mediator;

        public ShoppingCartService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetCartResponse> GetCart(GetCartRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<GetCartByUserIdResponse> GetCartByUserId(GetCartByUserIdRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<InsertItemToNewCartResponse> InsertItemToNewCart(InsertItemToNewCartRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<UpdateItemInCartResponse> UpdateItemInCart(UpdateItemInCartRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<DeleteItemResponse> DeleteItem(DeleteItemRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }

        public override async Task<CheckoutResponse> Checkout(CheckoutRequest request, ServerCallContext context)
        {
            return await _mediator.Send(request);
        }
    }
}
