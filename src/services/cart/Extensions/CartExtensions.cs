using System.Linq;
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
  }
}
