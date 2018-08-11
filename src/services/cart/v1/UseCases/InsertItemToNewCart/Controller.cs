using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class Controller : EvtControllerBase
  {
    public Controller(IMediator mediator) : base(mediator) { }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> Create(InsertItemToNewCartRequest request) =>
      await Eventor.SendStream<InsertItemToNewCartRequest, InsertItemToNewCartResponse>(request, x => x.Result);
  }
}
