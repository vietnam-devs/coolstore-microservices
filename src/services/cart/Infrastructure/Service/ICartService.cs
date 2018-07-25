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
    TaxCalculatorContext TaxCalculator { get; set; }
    Task<GetCartByIdResponse> GetCartByIdAsync(Guid id);
    Task<Domain.Cart> InsertItemToCartAsync(InsertItemToNewCartRequest request);
    Task<Domain.Cart> UpdateItemInCartAsync(UpdateItemInCartRequest request);
    Task<bool> RemoveItemInCartAsync(Guid cartId, Guid itemId);
    Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request);
  }
}
