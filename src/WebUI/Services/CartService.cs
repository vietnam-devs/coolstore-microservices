using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Model;

namespace WebUI.Services
{
  public class CartService
  {
    private readonly IEnumerable<CartModel> _carts;

    public CartService()
    {
      _carts = new List<CartModel>();
      _carts = _carts.Append(new CartModel
      {
        CartId = Guid.NewGuid(),
        Quantity = 10
      });
      _carts = _carts.Append(new CartModel
      {
        CartId = Guid.NewGuid(),
        Quantity = 5
      });
    }

    public Task<CartModel> CreateCart()
    {
      return Task.FromResult(new CartModel());
    }

    public Task<CartModel> GetCart(Guid cartId)
    {
      return Task.FromResult(_carts.FirstOrDefault(x => x.CartId == cartId));
    }

    public Task<CartModel> AddProductToCart(Guid cartId, Guid productId, int quantity)
    {
      return Task.FromResult(new CartModel());
    }

    public Task<CartModel> UpdateCart(Guid cartId, Guid productId, int quantity)
    {
      return Task.FromResult(new CartModel());
    }

    public Task<CartModel> RemoveFromCart(Guid cartId, Guid productId)
    {
      return Task.FromResult(new CartModel());
    }

    public Task Checkout(Guid cartId)
    {
      return Task.CompletedTask;
    }
  }
}
