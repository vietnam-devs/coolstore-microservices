using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Cart.Checkout;
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

    public async Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request)
    {
      var endPoint = $"{_cartServiceUri}/api/v1/carts/{request.Id}/checkout";
      RestClient.SetOpenTracingInfo(request.Headers);
      return await RestClient.PostAsync<CheckoutResponse>(endPoint, request);
    }

    public async Task<GetCartByIdResponse> CreateCartAsync(InsertItemToNewCartRequest request)
    {
      string endPoint = $"{_cartServiceUri}/api/v1/carts";
      RestClient.SetOpenTracingInfo(request.Headers);
      var response = await RestClient.PostAsync<GetCartByIdResponse>(endPoint, request);
      return response;
    }

    public async Task DeleteItemInCartAsync(DeleteItemInCartRequest request)
    {
      string deleteItemInCartEndPoint = $"{_cartServiceUri}/api/v1/carts/{request.Id}/items/{request.ProductId}";
      RestClient.SetOpenTracingInfo(request.Headers);
      bool result = await RestClient.DeleteAsync(deleteItemInCartEndPoint);
    }

    public async Task<GetCartByIdResponse> GetCartByIdAsync(GetCartByIdRequest request)
    {
      var getCartEndPoint = $"{_cartServiceUri}/api/v1/carts/{request.Id}";
      RestClient.SetOpenTracingInfo(request.Headers);
      var cart = await RestClient.GetAsync<GetCartByIdResponse>(getCartEndPoint);
      return cart;
    }

    public async Task<GetCartByIdResponse> UpdateCartAsync(UpdateItemInCartRequest request)
    {
      var endPoint = $"{_cartServiceUri}/api/v1/carts";
      RestClient.SetOpenTracingInfo(request.Headers);
      var response = await RestClient.PutAsync<GetCartByIdResponse>(endPoint, request);
      return response;
    }
  }
}
