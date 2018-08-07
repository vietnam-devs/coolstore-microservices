using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore.CleanArch;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public class CheckoutHandler : EventHandlerBase<CheckoutRequest, CheckoutResponse>
  {
    public CheckoutHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf)
      : base(uow, qrf)
    {
    }

    public override async Task<CheckoutResponse> Handle(CheckoutRequest request, CancellationToken cancellationToken)
    {
      var cartQueryRepository = QueryRepositoryFactory.QueryEfRepository<Domain.Cart>();
      var cartRepository = UnitOfWork.Repository<Domain.Cart>();

      var cart = await cartQueryRepository.GetFullCart(request.CartId);

      cart.IsCheckout = true;
      var checkoutCart = await cartRepository.UpdateAsync(cart);

      return new CheckoutResponse
      {
        IsSucceed = checkoutCart != null
      };
    }
  }
}
