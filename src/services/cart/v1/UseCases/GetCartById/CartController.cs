using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class CartController : EvtControllerBase
  {
    public CartController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    [Route("{id}")]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> Get([FromBody] GetCartRequest request) =>
      await Eventor.SendStream<GetCartRequest, GetCartResponse>(request, x => x.Result);
  }
}
