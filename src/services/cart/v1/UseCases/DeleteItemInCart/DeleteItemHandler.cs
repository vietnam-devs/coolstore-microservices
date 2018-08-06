using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.Infrastructure.Services;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Service;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public class DeleteItemHandler : CartServiceBase,
    IRequestHandler<DeleteItemRequest, DeleteItemResponse>,
    ICommandService, IQueryService
  {
    public DeleteItemHandler(
      ICatalogGateway catalogGateway,
      IUnitOfWorkAsync uow,
      IQueryRepositoryFactory queryRepositoryFactory)
      : base(catalogGateway)
    {
      UnitOfWork = uow;
      QueryRepositoryFactory = queryRepositoryFactory;
    }

    public PriceCalculatorContext PriceCalculatorContext { get; set; }
    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public IUnitOfWorkAsync UnitOfWork { get; }

    public override IEfQueryRepository<Domain.Cart> GetQueryRepository()
    {
      return QueryRepositoryFactory.QueryRepository<Domain.Cart>() as IEfQueryRepository<Domain.Cart>;
    }

    public async Task<DeleteItemResponse> Handle(DeleteItemRequest request, CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cartItemRepository = UnitOfWork.Repository<CartItem>();

      var cart = await GetCart(request.CartId);
      cart = await InitCart(cart);

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
