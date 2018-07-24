using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Shared.Cart.GetCartById;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;

namespace VND.CoolStore.Services.Cart.Infrastructure.Service
{
  public interface ICartService
  {
    TaxCalculatorContext TaxCalculator { get; set; }
    Task<GetCartByIdResponse> GetCartById(Guid id);
    Task<Domain.Cart> InsertItemToCart(InsertItemToNewCartRequest request);
    Task<Domain.Cart> UpdateItemInCart(UpdateItemInCartRequest request);
    Task<bool> RemoveItemInCart(Guid cartId, Guid itemId);
  }
}
