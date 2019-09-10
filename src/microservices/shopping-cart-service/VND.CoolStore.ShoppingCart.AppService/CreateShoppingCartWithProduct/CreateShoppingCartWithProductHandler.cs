using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.CreateShoppingCartWithProduct
{
    public class CreateShoppingCartWithProductHandler : IRequestHandler<InsertItemToNewCartRequest, InsertItemToNewCartResponse>
    {
        public Task<InsertItemToNewCartResponse> Handle(InsertItemToNewCartRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new InsertItemToNewCartResponse());
        }
    }
}
