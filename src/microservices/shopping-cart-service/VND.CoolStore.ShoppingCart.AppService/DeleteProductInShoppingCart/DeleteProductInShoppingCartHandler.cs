using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.DeleteProductInShoppingCart
{
    public class DeleteProductInShoppingCartHandler : IRequestHandler<DeleteItemRequest, DeleteItemResponse>
    {
        public Task<DeleteItemResponse> Handle(DeleteItemRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new DeleteItemResponse());
        }
    }
}
