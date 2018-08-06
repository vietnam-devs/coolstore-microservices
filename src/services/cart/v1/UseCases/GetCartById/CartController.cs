using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Cart.UseCases.v1.Services;
using ControllerBase = VND.FW.Infrastructure.AspNetCore.ControllerBase;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class CartController : ControllerBase
  {
    private readonly IMediator _eventAggregator;
    private readonly GetCartPresenter _presenter;

    public CartController(
      IMediator eventAggregator,
      GetCartPresenter presenter)
    {
      _eventAggregator = eventAggregator;
      _presenter = presenter;
    }

    [HttpGet]
    [Route("{id}")]
    // [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> Get(Guid id)
    {
      //TODO: stupid code
      if (id == Guid.Empty)
        return null;

      var result = await _eventAggregator.Send(
        new GetCartRequest
        {
          CartId = id
        });

      return _presenter.Populate(result);
    }
  }
}
