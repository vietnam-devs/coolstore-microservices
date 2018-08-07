using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class CartController : EvtControllerBase
  {
    public CartController(IMediator mediator) : base(mediator) { }

    [HttpPut]
    [Route("{cartId:guid}/checkout")]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> CheckoutCart([FromBody] CheckoutRequest request) =>
      await Eventor.SendStream<CheckoutRequest, CheckoutResponse>(request, x => x.IsSucceed);
  }
}
