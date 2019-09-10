using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.CheckOutShoppingCart
{
    public class CheckOutShoppingCartHandler : IRequestHandler<CheckoutRequest, CheckoutResponse>
    {
        public Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CheckoutResponse());
        }
    }
}
