using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Extensions;
using VND.CoolStore.Services.Cart.v1.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public class RequestHandler : TxRequestHandlerBase<DeleteItemRequest, DeleteItemResponse>
  {
    private readonly ICatalogGateway _catalogGateway;
    public RequestHandler(ICatalogGateway cgw, IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf)
      : base(uow, qrf)
    {
      _catalogGateway = cgw;
    }

    public override async Task<DeleteItemResponse> Handle(DeleteItemRequest request, CancellationToken cancellationToken)
    {
      var cartCommander = UnitOfWork.Repository<Domain.Cart>();
      var cartItemCommander = UnitOfWork.Repository<CartItem>();
      var cartQuery = QueryRepositoryFactory.QueryEfRepository<Domain.Cart>();

      var cart = await cartQuery
        .GetFullCartAsync(request.CartId)
        .ToObservable()
        .SelectMany(c => c.InitCart(_catalogGateway));

      var cartItem = cart.CartItems.FirstOrDefault(x => x.Product.ProductId == request.ProductId);
      if (cartItem == null)
      {
        throw new Exception($"Could not find Product {request.ProductId}.");
      }

      await cartItemCommander.DeleteAsync(cartItem);
      await UnitOfWork.SaveChangesAsync(cancellationToken);

      cart = cart.RemoveCartItem(cartItem.Id);
      await cartCommander.UpdateAsync(cart);
      await UnitOfWork.SaveChangesAsync(cancellationToken);

      return new DeleteItemResponse { ProductId = cartItem.Product.ProductId };
    }
  }
}
