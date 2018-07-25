using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Service;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Cart.Checkout;
using VND.CoolStore.Shared.Cart.DeleteItemInCart;
using VND.CoolStore.Shared.Cart.GetCartById;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/carts")]
  public class CartController : FW.Infrastructure.AspNetCore.ControllerBase
  {
    private readonly ICartService _cartService;
    private readonly ILogger<CartController> _logger;

    public CartController(ICartService cartService, ILoggerFactory logger)
    {
      _cartService = cartService;
      _logger = logger.CreateLogger<CartController>();
    }

    [HttpGet]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}")]
    public async Task<ActionResult<GetCartByIdResponse>> GetCart(Guid cartId)
    {
      var cart = await _cartService.GetCartByIdAsync(
        new Shared.Cart.GetCartById.GetCartByIdRequest
        {
          Id = cartId,
          Headers = HttpContext.Request.GetOpenTracingInfo()
        });

      return Ok(cart);
    }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    public async Task<ActionResult<GetCartByIdResponse>> CreateCart([FromBody] InsertItemToNewCartRequest request)
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

      request.Headers = HttpContext.Request.GetOpenTracingInfo();
      var response = await _cartService.CreateCartAsync(request);
      return Ok(response);
    }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/checkout")]
    public async Task<ActionResult<CheckoutResponse>> Checkout(Guid cartId)
    {
      var request = new CheckoutRequest { Id = cartId };
      request.Headers = HttpContext.Request.GetOpenTracingInfo();
      var response = await _cartService.CheckoutAsync(request);
      return Ok(response);
    }

    [HttpPut]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    public async Task<ActionResult<GetCartByIdResponse>> UpdateCart([FromBody] UpdateItemInCartRequest request)
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

      request.Headers = HttpContext.Request.GetOpenTracingInfo();
      var response = await _cartService.UpdateCartAsync(request);
      return Ok(response);
    }

    [HttpDelete]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/item/{itemId:guid}")]
    public async Task<ActionResult<bool>> DeleteItemInCart(Guid cartId, Guid itemId)
    {
      await _cartService.DeleteItemInCartAsync(new DeleteItemInCartRequest
      {
        Id = cartId,
        ItemId = itemId,
        Headers = HttpContext.Request.GetOpenTracingInfo()
      });

      //TODO: temporary hard code here
      return Ok(true);
    }
  }
}
