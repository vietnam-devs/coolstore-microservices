using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Cart.DeleteItemInCart;
using VND.CoolStore.Shared.Cart.GetCartById;
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
      IConfiguration config,
      IHostingEnvironment env) : base(rest)
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
      RestClient.SetOpenTracingInfo(request.Headers);
      InsertItemToNewCartResponse response = await RestClient.PostAsync<InsertItemToNewCartResponse>(endPoint, request);
      return response;
    }

    public async Task DeleteItemInCart(DeleteItemInCartRequest request)
    {
      string deleteItemInCartEndPoint = $"{_cartServiceUri}/api/v1/carts/{request.Id}/items/{request.ItemId}";
      RestClient.SetOpenTracingInfo(request.Headers);
      bool result = await RestClient.DeleteAsync(deleteItemInCartEndPoint);
    }

    public async Task<CartModel> GetCartByIdAsync(GetCartByIdRequest request)
    {
      string getCartEndPoint = $"{_cartServiceUri}/api/v1/carts/{request.Id}";
      RestClient.SetOpenTracingInfo(request.Headers);
      CartModel cart = await RestClient.GetAsync<CartModel>(getCartEndPoint);
      return cart;
    }

    public async Task<UpdateItemInCartResponse> UpdateCart(UpdateItemInCartRequest request)
    {
      string endPoint = $"{_cartServiceUri}/api/v1/carts/update-cart";
      RestClient.SetOpenTracingInfo(request.Headers);
      UpdateItemInCartResponse response = await RestClient.PutAsync<UpdateItemInCartResponse>(endPoint, request);
      return response;
    }
  }
}
