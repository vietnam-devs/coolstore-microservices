using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore.Authz;

namespace VND.CoolStore.Services.Cart.v2
{
  [ApiVersion("2.0")]
  [Route("api/carts")]
  public class CartController : Controller
  {
    /*private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
      _cartService = cartService;
    }*/

    [HttpGet]
    [Route("{id}")]
    [Auth(Policy = "access_cart_api")]
    public ActionResult<string> Get(Guid id, CancellationToken cancellationToken)
    {
      return Ok("2.0");
    }
  }
}
