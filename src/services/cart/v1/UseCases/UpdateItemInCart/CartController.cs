using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VND.FW.Infrastructure.AspNetCore;
using ControllerBase = VND.FW.Infrastructure.AspNetCore.ControllerBase;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class CartController : ControllerBase
  {
    private readonly IMediator _eventAggregator;
    private readonly UpdateItemPresenter _presenter;

    public CartController(
      IMediator eventAggregator,
      UpdateItemPresenter presenter)
    {
      _eventAggregator = eventAggregator;
      _presenter = presenter;
    }

    [HttpPut]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> Put([FromBody] UpdateItemInCartRequest request)
    {
      var result = await _eventAggregator.Send(request);
      return _presenter.Populate(result);
    }
  }
}
