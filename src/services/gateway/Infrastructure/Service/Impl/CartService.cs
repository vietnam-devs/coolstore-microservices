using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.ApiGateway.Infrastructure.Service.Impl
{
  public class CartService : ProxyServiceBase, ICartService
  {
    private readonly string _cartServiceUri;

    public CartService(
      RestClient rest,
      IHttpContextAccessor httpContextAccessor,
      IConfiguration config,
      IHostingEnvironment env) : base(rest, httpContextAccessor)
    {
      _cartServiceUri = config.GetHostUri(env, "Cart");
    }

    public Task CheckoutAsync(Guid cartId)
    {
      throw new NotImplementedException();
    }

    public async Task<InsertItemToNewCartResponse> CreateCartAsync(InsertItemToNewCartRequest request)
    {
      string endPoint = $"{_cartServiceUri}/api/v1/carts/new-cart";
      InsertItemToNewCartResponse response = await RestClient.PostAsync<InsertItemToNewCartResponse>(endPoint, request);
      return response;
    }

    public async Task DeleteItemInCart(Guid cartId, Guid itemId)
    {
      string deleteItemInCartEndPoint = $"{_cartServiceUri}/api/v1/carts/{cartId}/items/{itemId}";
      bool result = await RestClient.DeleteAsync(deleteItemInCartEndPoint);
    }

    public async Task<CartModel> GetCartByIdAsync(Guid cartId)
    {
      string getCartEndPoint = $"{_cartServiceUri}/api/v1/carts/{cartId}";
      CartModel cart = await RestClient.GetAsync<CartModel>(getCartEndPoint);
      return cart;
    }

    public async Task<UpdateItemInCartResponse> UpdateCart(UpdateItemInCartRequest request)
    {
      string endPoint = $"{_cartServiceUri}/api/v1/carts/update-cart";
      UpdateItemInCartResponse response = await RestClient.PutAsync<UpdateItemInCartResponse>(endPoint, request);
      return response;
    }
  }
}
