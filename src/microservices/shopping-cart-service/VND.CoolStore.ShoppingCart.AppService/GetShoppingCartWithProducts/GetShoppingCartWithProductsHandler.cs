using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.GetShoppingCartWithProducts
{
    public class GetShoppingCartWithProductsHandler : IRequestHandler<GetCartRequest, GetCartResponse>
    {
        public Task<GetCartResponse> Handle(GetCartRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetCartResponse { Result = new CartDto() });
        }
    }
}
