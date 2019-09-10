using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.UpdateAmountOfProductInShoppingCart
{
    public class UpdateAmountOfProductInShoppingCartHandler : IRequestHandler<UpdateItemInCartRequest, UpdateItemInCartResponse>
    {
        public Task<UpdateItemInCartResponse> Handle(UpdateItemInCartRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new UpdateItemInCartResponse());
        }
    }
}
