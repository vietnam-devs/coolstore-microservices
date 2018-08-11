using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class Controller : EvtControllerBase
  {
    public Controller(IMediator mediator) : base(mediator) { }

    [HttpPut]
    [Route("{cartId:guid}/checkout")]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> CheckoutCart(Guid cartId)
    {
      return await Eventor.SendStream<CheckoutRequest, CheckoutResponse>(
        new CheckoutRequest { CartId = cartId },
        x => x.IsSucceed);
    }
  }
}
