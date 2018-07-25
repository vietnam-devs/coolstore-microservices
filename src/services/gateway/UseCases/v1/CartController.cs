using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Service;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Cart.DeleteItemInCart;
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
    public async Task<ActionResult<CartModel>> GetCart(Guid cartId)
    {

      CartModel cart = await _cartService.GetCartByIdAsync(
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

      request.Headers = HttpContext.Request.GetOpenTracingInfo();
      InsertItemToNewCartResponse response = await _cartService.CreateCartAsync(request);
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

      request.Headers = HttpContext.Request.GetOpenTracingInfo();
      request.CartId = cartId;
      request.ItemId = itemId;

      UpdateItemInCartResponse response = await _cartService.UpdateCart(request);
      return Ok(response);
    }

    [HttpDelete]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/item/{itemId:guid}")]
    public async Task<ActionResult<bool>> DeleteItemInCart(Guid cartId, Guid itemId)
    {
      await _cartService.DeleteItemInCart(new DeleteItemInCartRequest
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
