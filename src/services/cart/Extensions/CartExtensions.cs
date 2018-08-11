using System;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Dtos;

namespace VND.CoolStore.Services.Cart.Extensions
{
  public static class CartExtensions
  {
    public static CartDto ToDto(this Domain.Cart cart)
    {
      return new CartDto
      {
        Id = cart.Id,
        CartTotal = cart.CartTotal,
        CartItemTotal = cart.CartItemTotal,
        CartItemPromoSavings = cart.CartItemPromoSavings,
        ShippingPromoSavings = cart.ShippingPromoSavings,
        ShippingTotal = cart.ShippingTotal,
        IsCheckout = cart.IsCheckout,
        Items = cart.CartItems.Select(cc => new CartDto.CartItemDto
        {
          ProductId = cc.Product.ProductId,
          ProductName = cc.Product.Name,
          Price = cc.Price,
          Quantity = cc.Quantity,
          PromoSavings = cc.PromoSavings
        }).ToList()
      };
    }

    public static async Task<Domain.Cart> InitCart(this Domain.Cart cart, ICatalogGateway catalogGateway, bool isPopulatePrice = false)
    {
      if (cart == null)
      {
        cart = new Domain.Cart();
      }

      if (isPopulatePrice == false)
      {
        cart.CartItemPromoSavings = 0;
        cart.CartTotal = 0;
        cart.ShippingPromoSavings = 0;
        cart.ShippingTotal = 0;
        cart.CartItemTotal = 0;
      }

      if (cart.CartItems == null)
        return cart;
      foreach (var item in cart.CartItems)
      {
        var product = await catalogGateway.GetProductByIdAsync(item.Product.ProductId);

        if (product == null)
        {
          throw new Exception("Could not find product.");
        }

        item.Product = new Product(product.Id, product.Name, product.Price, product.Desc);
        item.Price = product.Price;
        item.PromoSavings = 0;
      }

      return cart;
    }
  }
}
