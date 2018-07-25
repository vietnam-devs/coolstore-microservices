using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Service;
using VND.CoolStore.Shared.Cart.Checkout;
using VND.CoolStore.Shared.Cart.GetCartById;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;

namespace VND.CoolStore.Services.Cart.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/carts")]
  public class CartController : FW.Infrastructure.AspNetCore.ControllerBase
  {
    private readonly ICartService _cartService;

    public CartController(ICartService cartService, NoTaxCaculator taxCaculator)
    {
      _cartService = cartService;
      _cartService.TaxCalculator = taxCaculator;
    }

    [HttpGet(Name = nameof(GetCartById))]
    [Route("{id}")]
    public async Task<GetCartByIdResponse> GetCartById(Guid id)
    {
      return await _cartService.GetCartByIdAsync(id);
    }

    [HttpPost]
    [Route("insert-item")]
    public async Task<GetCartByIdResponse> InsertItemToCart([FromBody] InsertItemToNewCartRequest request)
    {
      var cart = await _cartService.InsertItemToCartAsync(request);
      return await _cartService.GetCartByIdAsync(cart.Id);
    }

    [HttpPut]
    [Route("update-item")]
    public async Task<GetCartByIdResponse> UpdateItemInCart([FromBody] UpdateItemInCartRequest request)
    {
      var cart = await _cartService.UpdateItemInCartAsync(request);
      return await _cartService.GetCartByIdAsync(cart.Id);
    }

    [HttpDelete]
    [Route("{cartId:guid}/items/{itemId:guid}")]
    public async Task<bool> RemoveItemInCart(Guid cartId, Guid itemId)
    {
      return await _cartService.RemoveItemInCartAsync(cartId, itemId);
    }

    [HttpPost]
    [Route("{cartId:guid}/checkout")]
    public async Task<CheckoutResponse> CheckoutCart(Guid cartId)
    {
      return await _cartService.CheckoutAsync(new CheckoutRequest { Id = cartId });
    }
  }
}
