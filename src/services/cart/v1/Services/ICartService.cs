using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Shared.Services;
using VND.CoolStore.Services.Cart.v1.UseCases.Checkout;
using VND.CoolStore.Services.Cart.v1.UseCases.GetCartById;
using VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart;
using VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart;

namespace VND.CoolStore.Services.Cart.UseCases.v1.Services
{
  public interface ICartService
  {
    PriceCalculatorContext PriceCalculatorContext { get; set; }
    Task<GetCartByIdResponse> GetCartByIdAsync(Guid id);
    Task<GetCartByIdResponse> InsertItemToCartAsync(InsertItemToNewCartRequest request);
    Task<GetCartByIdResponse> UpdateItemInCartAsync(UpdateItemInCartRequest request);
    Task<bool> RemoveItemInCartAsync(Guid cartId, Guid itemId);
    Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request);
  }
}
