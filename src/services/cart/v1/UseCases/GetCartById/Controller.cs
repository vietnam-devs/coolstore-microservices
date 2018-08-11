using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class Controller : EvtControllerBase
  {
    public Controller(IMediator mediator) : base(mediator) { }

    [HttpGet]
    [Route("{id}")]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> Get(Guid id)
    {
      return await Eventor.SendStream<GetCartRequest, GetCartResponse>(
        new GetCartRequest { CartId = id },
        x => x.Result);
    }
  }
}
