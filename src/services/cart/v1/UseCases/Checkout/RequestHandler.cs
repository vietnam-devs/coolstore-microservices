using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Cart.v1.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public class RequestHandler : TxRequestHandlerBase<CheckoutRequest, CheckoutResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf)
      : base(uow, qrf)
    {
    }

    public override async Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken)
    {
      var cartCommander = UnitOfWork.Repository<Domain.Cart>();
      var cartQuery = QueryRepositoryFactory.QueryEfRepository<Domain.Cart>();

      var cart = await cartQuery.GetFullCartAsync(request.CartId);

      cart.IsCheckout = true;
      var checkoutCart = await cartCommander.UpdateAsync(cart);
      await UnitOfWork.SaveChangesAsync(cancellationToken);

      return new CheckoutResponse
      {
        IsSucceed = checkoutCart != null
      };
    }
  }
}
