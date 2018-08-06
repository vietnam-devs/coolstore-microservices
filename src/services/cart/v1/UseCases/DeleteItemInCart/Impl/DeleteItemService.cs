using System;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Service;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart.Impl
{
  public class DeleteItemService : CartServiceBase, IDeleteItemService, ICommandService, IQueryService
  {
    public DeleteItemService(
      ICatalogGateway catalogGateway,
      IUnitOfWorkAsync uow,
      IQueryRepositoryFactory queryRepositoryFactory)
      : base(catalogGateway)
    {
      UnitOfWork = uow;
      QueryRepositoryFactory = queryRepositoryFactory;
    }

    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public IUnitOfWorkAsync UnitOfWork { get; }

    public override IEfQueryRepository<Domain.Cart> GetQueryRepository()
    {
      return QueryRepositoryFactory.QueryRepository<Domain.Cart>() as IEfQueryRepository<Domain.Cart>;
    }

    public async Task<bool> Execute(Guid cartId, Guid productId)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cartItemRepository = UnitOfWork.Repository<CartItem>();

      var cart = await GetCart(cartId);
      cart = await InitCart(cart);

      var cartItem = cart.CartItems.FirstOrDefault(x => x.Product.ProductId == productId);
      if (cartItem == null)
      {
        throw new Exception($"Could not find CartItem {cartItem.Id}");
      }

      cart = cart.RemoveCartItem(cartItem.Id);
      var isSucceed = await cartRepository.UpdateAsync(cart) != null;
      await cartItemRepository.DeleteAsync(cartItem);

      return UnitOfWork.Commit();
    }
  }
}
