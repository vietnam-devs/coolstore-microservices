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
        CartId = new Guid("{32EF8824-4C37-47BD-91DE-034DC4BAFF0C}"),
        CartItemTotal = 1000D,
        ShippingTotal = 1000D,
        CartItemPromoSavings = 10D,
        ShippingPromoSavings = 5D,
        Items = new List<CartItemModel>
        {
          new CartItemModel
          {
            ProductId = Guid.NewGuid(),
            Name = "Product 1",
            Quantity = 10,
            Price = 150
          },
          new CartItemModel
          {
            ProductId = Guid.NewGuid(),
            Name = "Product 2",
            Quantity = 20,
            Price = 100
          }
        }
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

    public async Task<CartModel> AddProductToCart(Guid cartId, Guid productId, int quantity)
    {
      var cart = await GetCart(cartId);

      cart.Items.Add(new CartItemModel
      {
        ProductId =  productId,
        Quantity = quantity,
        Price = 10D,
        Name = "dummy"
      });

      return cart;
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
