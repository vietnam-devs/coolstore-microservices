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

    public override async Task<CheckoutResponse> TxHandle(CheckoutRequest request, CancellationToken cancellationToken)
    {
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();

      var cart = await QueryRepositoryFactory
        ?.QueryEfRepository<Domain.Cart>()
        ?.GetFullCart(request.CartId);

      cart.IsCheckout = true;
      var checkoutCart = await cartRepository.UpdateAsync(cart);

      return new CheckoutResponse
      {
        IsSucceed = checkoutCart != null
      };
    }
  }
}
