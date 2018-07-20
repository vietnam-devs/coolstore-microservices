using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Extensions;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;
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
      CartModel cart = await RestClient.GetAsync<CartModel>(getCartEndPoint);

      return Ok(cart);
    }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    public async Task<ActionResult<InsertItemToNewCartResponse>> CreateCart([FromBody] InsertItemToNewCartRequest request)
    {
      InitRestClientWithOpenTracing();

      if (!ModelState.IsValid)
      {
        return BadRequest(new
        {
          Message = "ModelState is not valid. Check the ModelState property for specific errors.",
          StatusCode = 400,
          ModelState
        });
      }

      string endPoint = $"{_cartServiceUri}/api/v1/carts/new-cart";
      InsertItemToNewCartResponse response = await RestClient.PostAsync<InsertItemToNewCartResponse>(endPoint, request);

      return Ok(response);
    }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/checkout")]
    public IActionResult Checkout(Guid cartId)
    {
      InitRestClientWithOpenTracing();

      return Ok(cartId);
    }

    [HttpPut]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/item/{itemId:guid}")]
    public async Task<ActionResult<UpdateItemInCartResponse>> UpdateCart(Guid cartId, Guid itemId, [FromBody] UpdateItemInCartRequest request)
    {
      InitRestClientWithOpenTracing();

      if (!ModelState.IsValid)
      {
        return BadRequest(new
        {
          Message = "ModelState is not valid. Check the ModelState property for specific errors.",
          StatusCode = 400,
          ModelState
        });
      }

      string endPoint = $"{_cartServiceUri}/api/v1/carts/update-cart";
      UpdateItemInCartResponse response = await RestClient.PutAsync<UpdateItemInCartResponse>(endPoint, request);

      return Ok(response);
    }

    [HttpDelete]
    [Auth(Policy = "access_cart_api")]
    [SwaggerOperation(Tags = new[] { "cart-service" })]
    [Route("{cartId:guid}/item/{itemId:guid}")]
    public async Task<ActionResult<bool>> DeleteItemInCart(Guid cartId, Guid itemId)
    {
      InitRestClientWithOpenTracing();

      string deleteItemInCartEndPoint = $"{_cartServiceUri}/api/v1/carts/{cartId}/items/{itemId}";
      bool result = await RestClient.DeleteAsync(deleteItemInCartEndPoint);

      return Ok(result);
    }
  }
}
