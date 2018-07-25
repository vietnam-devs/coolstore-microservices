using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Cart.DeleteItemInCart;
using VND.CoolStore.Shared.Cart.GetCartById;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;

namespace VND.CoolStore.Services.ApiGateway.Infrastructure.Service
{
  public interface ICartService
  {
    Task<CartModel> GetCartByIdAsync(GetCartByIdRequest request);
    Task<InsertItemToNewCartResponse> CreateCartAsync(InsertItemToNewCartRequest request);
    Task<UpdateItemInCartResponse> UpdateCart(UpdateItemInCartRequest request);
    Task CheckoutAsync(Guid cartId);
    Task DeleteItemInCart(DeleteItemInCartRequest request);
  }
}
