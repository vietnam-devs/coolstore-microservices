using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using WebUI.Model;

namespace WebUI.Services
{
  public class CartService : BaseService
  {
    private readonly string _cartUrl;

    public CartService(AppState appState, ConfigModel config, HttpClient httpClient)
      : base(appState, config, httpClient)
    {
      _cartUrl = $"{config.CartService}";
    }

    public Task<CartModel> CreateCart()
    {
      return Task.FromResult(new CartModel());
    }

    public async Task<CartModel> GetCart(Guid cartId)
    {
      await SetHeaderToken();
      return await RestClient.GetJsonAsync<CartModel>($"{_cartUrl}/api/carts/{cartId}");
    }

    public async Task<CartModel> AddProductToCart(Guid? cartId, Guid productId, int quantity)
    {
      await SetHeaderToken();

      if (cartId == Guid.Empty || !cartId.HasValue)
      {
        return await RestClient.PostJsonAsync<CartModel>(
          $"{_cartUrl}/api/carts",
          new
          {
            productId,
            quantity
          });
      }

      return await RestClient.PutJsonAsync<CartModel>(
        $"{_cartUrl}/api/carts",
        new
        {
          CartId = cartId.Value,
          productId,
          quantity
        });
    }

    public async Task<CartModel> UpdateCart(Guid cartId, Guid productId, int quantity)
    {
      await SetHeaderToken();
      return await RestClient.PutJsonAsync<CartModel>(
        $"{_cartUrl}/api/carts",
        new
        {
          CartId = cartId,
          productId,
          quantity
        });
    }

    public async Task RemoveFromCart(Guid cartId, Guid productId)
    {
      await SetHeaderToken();
      await RestClient.DeleteAsync($"{_cartUrl}/api/carts/{cartId}/items/{productId}");
    }

    public async Task Checkout(Guid cartId)
    {
      await SetHeaderToken();
      await RestClient.DeleteAsync($"{_cartUrl}/api/carts/{cartId}/checkout");
    }
  }
}
