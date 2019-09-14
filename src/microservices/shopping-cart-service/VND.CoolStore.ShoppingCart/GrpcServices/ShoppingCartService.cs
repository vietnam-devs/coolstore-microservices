using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;
using static VND.CoolStore.ShoppingCart.DataContracts.V1.ShoppingCart;

namespace VND.CoolStore.ShoppingCart.GrpcServices
{
    public class ShoppingCartService : ShoppingCartBase
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
