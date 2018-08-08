using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Extensions;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.CleanArch;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public class CartRequestHandler : TxRequestHandlerBase<DeleteItemRequest, DeleteItemResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    public CartRequestHandler(ICatalogGateway cgw, IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf)
      : base(uow, qrf)
    {
      _catalogGateway = cgw;
    }

    public override async Task<DeleteItemResponse> TxHandle(DeleteItemRequest request, CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cartItemRepository = UnitOfWork.Repository<CartItem>();

      var cart = await QueryRepositoryFactory
        ?.QueryEfRepository<Domain.Cart>()
        ?.GetFullCart(request.CartId)
        ?.ToObservable()
        ?.SelectMany(c => c.InitCart(_catalogGateway));

      var cartItem = cart.CartItems.FirstOrDefault(x => x.Product.ProductId == request.ProductId);
      if (cartItem == null)
      {
        throw new Exception($"Could not find Product {request.ProductId}.");
      }

      cart = cart.RemoveCartItem(cartItem.Id);
      var isSucceed = await cartRepository.UpdateAsync(cart) != null;
      await cartItemRepository.DeleteAsync(cartItem);

      return new DeleteItemResponse { ProductId = cartItem.Product.ProductId };
    }
  }
}
