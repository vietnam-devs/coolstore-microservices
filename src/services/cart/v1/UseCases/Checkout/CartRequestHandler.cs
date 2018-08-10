using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.v1.Extensions;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.Fw.Infrastructure.AspNetCore.CleanArch;
using VND.Fw.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public class CartRequestHandler : TxRequestHandlerBase<CheckoutRequest, CheckoutResponse>
  {
    public CartRequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf)
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
