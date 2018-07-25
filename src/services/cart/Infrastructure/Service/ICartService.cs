using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Shared.Cart.Checkout;
using VND.CoolStore.Shared.Cart.GetCartById;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;

namespace VND.CoolStore.Services.Cart.Infrastructure.Service
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
