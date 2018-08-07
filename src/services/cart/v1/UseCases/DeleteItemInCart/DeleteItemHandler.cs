using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.CleanArch;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public class DeleteItemHandler : EventHandlerBase<DeleteItemRequest, DeleteItemResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    public DeleteItemHandler(ICatalogGateway cgw, IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf)
      : base(uow, qrf)
    {
      _catalogGateway = cgw;
    }

    public override async Task<DeleteItemResponse> Handle(DeleteItemRequest request, CancellationToken cancellationToken)
    {
      var cartQueryRepository = QueryRepositoryFactory.QueryEfRepository<Domain.Cart>();
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cartItemRepository = UnitOfWork.Repository<CartItem>();

      var cart = await cartQueryRepository.GetFullCart(request.CartId);
      cart = await cart.InitCart(_catalogGateway);

      var cartItem = cart.CartItems.FirstOrDefault(x => x.Product.ProductId == request.ProductId);
      if (cartItem == null)
      {
        throw new Exception($"Could not find CartItem {cartItem.Id}");
      }

      cart = cart.RemoveCartItem(cartItem.Id);
      var isSucceed = await cartRepository.UpdateAsync(cart) != null;
      await cartItemRepository.DeleteAsync(cartItem);

      UnitOfWork.Commit();

      return new DeleteItemResponse { ProductId = cartItem.Product.ProductId };
    }
  }
}
