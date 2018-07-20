using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Extensions;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/carts")]
  public class CartController : ProxyControllerBase
  {
    private readonly string _cartServiceUri;
    public CartController(RestClient rest, IConfiguration config, IHostingEnvironment env) : base(rest)
    {
      _cartServiceUri = config.GetHostUri(env, "Cart");
    }

    [HttpGet]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}")]
    public async Task<ActionResult<CartModel>> GetCart(Guid cartId)
    {
      InitRestClientWithOpenTracing();

      string getCartEndPoint = $"{_cartServiceUri}/api/v1/carts/{cartId}";
      CartModel product = await RestClient.GetAsync<CartModel>(getCartEndPoint);

      return Ok(cartId);
    }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    public IActionResult CreateCart([FromBody] CreateCartModel cart)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new
        {
          Message = "ModelState is not valid. Check the ModelState property for specific errors.",
          StatusCode = 400,
          ModelState
        });
      }

      return Ok(cart);
    }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/checkout")]
    public IActionResult Checkout(Guid cartId)
    {
      return Ok(cartId);
    }

    [HttpPut]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/item/{itemId:guid}")]
    public IActionResult UpdateCart(Guid cartId, Guid itemId, [FromBody] UpdateCartModel updateModel)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(new
        {
          Message = "ModelState is not valid. Check the ModelState property for specific errors.",
          StatusCode = 400,
          ModelState
        });
      }

      return Ok();
    }

    [HttpDelete]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/item/{itemId:guid}")]
    public IActionResult DeleteCart(Guid cartId, Guid itemId)
    {
      return Ok();
    }
  }
}
