using System;
using Microsoft.AspNetCore.Mvc;
using VND.Fw.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.Cart.v2
{
  [ApiVersion("2.0")]
  [Route("api/carts")]
  public class CartController : Controller
  {
    /*private readonly ICartService _cartService;

    public CartController(ICartService cartService, TenPercentTaxCalculator taxCaculator)
    {
      _cartService = cartService;
      _cartService.PriceCalculatorContext = taxCaculator;
    }*/

    [HttpGet]
    [Route("{id}")]
    [Auth(Policy = "access_cart_api")]
    public ActionResult<string> Get(Guid id)
    {
      return Ok("2.0");
    }
  }
}
