using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Service;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public class CheckoutService : CartServiceBase, ICheckoutService, ICommandService, IQueryService
  {
    public CheckoutService(
      ICatalogGateway catalogGateway,
      IUnitOfWorkAsync uow,
      IQueryRepositoryFactory queryRepositoryFactory)
      : base(catalogGateway)
    {
      UnitOfWork = uow;
      QueryRepositoryFactory = queryRepositoryFactory;
    }

    public IUnitOfWorkAsync UnitOfWork { get; }

    public IQueryRepositoryFactory QueryRepositoryFactory { get; }

    public override IEfQueryRepository<Domain.Cart> GetQueryRepository()
    {
      return QueryRepositoryFactory.QueryRepository<Domain.Cart>() as IEfQueryRepository<Domain.Cart>;
    }

    public async Task<CheckoutResponse> Execute(CheckoutRequest request)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cart = await GetCart(request.Id);

      cart.IsCheckout = true;
      var checkoutCart = await cartRepository.UpdateAsync(cart);
      return new CheckoutResponse
      {
        IsSucceed = checkoutCart != null
      };
    }
  }
}
