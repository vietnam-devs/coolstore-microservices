using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Service;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/carts")]
  public class CartController : FW.Infrastructure.AspNetCore.ControllerBase
  {
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
      _cartService = cartService;
    }

    [HttpGet]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}")]
    public async Task<ActionResult<CartModel>> GetCart(Guid cartId)
    {
      var cart = await _cartService.GetCartByIdAsync(cartId);
      return Ok(cart);
    }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    public async Task<ActionResult<InsertItemToNewCartResponse>> CreateCart([FromBody] InsertItemToNewCartRequest request)
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

      var response = await _cartService.CreateCartAsync(request);
      return Ok(response);
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
    public async Task<ActionResult<UpdateItemInCartResponse>> UpdateCart(Guid cartId, Guid itemId, [FromBody] UpdateItemInCartRequest request)
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

      request.CartId = cartId;
      request.ItemId = itemId;

      var response = await _cartService.UpdateCart(request);
      return Ok(response);
    }

    [HttpDelete]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/item/{itemId:guid}")]
    public async Task<ActionResult<bool>> DeleteItemInCart(Guid cartId, Guid itemId)
    {
      await _cartService.DeleteItemInCart(cartId, itemId);

      //TODO: temporary hard code here
      return Ok(true);
    }
  }
}
