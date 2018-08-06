using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Service;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public class CheckoutHandler : CartServiceBase,
    IRequestHandler<CheckoutRequest, CheckoutResponse>,
    ICommandService, IQueryService
  {
    public CheckoutHandler(
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

    public async Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();
      var cart = await GetCart(request.CartId);

      cart.IsCheckout = true;
      var checkoutCart = await cartRepository.UpdateAsync(cart);
      return new CheckoutResponse
      {
        IsSucceed = checkoutCart != null
      };
    }
  }
}
