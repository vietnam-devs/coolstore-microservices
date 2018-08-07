using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain.Dtos;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class Cart : EntityBase
  {
    internal Cart() : base(Guid.NewGuid())
    {
    }

    public Cart(Guid id) : base(id)
    {
    }

    [Required]
    public double CartItemTotal { get; set; }

    [Required]
    public double CartItemPromoSavings { get; set; }

    [Required]
    public double ShippingTotal { get; set; }

    [Required]
    public double ShippingPromoSavings { get; set; }

    [Required]
    public double CartTotal { get; set; }

    [Required]
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    public bool IsCheckout { get; set; }

    public Cart InsertItemToCart(CartItem item)
    {
      CartItems.Add(item);
      return this;
    }

    public Cart RemoveCartItem(Guid itemId)
    {
      CartItems = CartItems.Where(y => !CartItems.Any(x => x.Id == itemId)).ToList();
      return this;
    }
  }

  public static class CartExtensions
  {
    public static CartDto ToCartDto(this Cart cart)
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
        Items = cart.CartItems.Select(cc =>
        {
          return new CartDto.CartItemDto
          {
            ProductId = cc.Product.ProductId,
            ProductName = cc.Product.Name,
            Price = cc.Price,
            Quantity = cc.Quantity,
            PromoSavings = cc.PromoSavings
          };
        }).ToList()
      };
    }

    public static async Task<Cart> InitCart(this Cart cart, ICatalogGateway catalogGateway, bool isPopulatePrice = false)
    {
      if (cart == null)
      {
        cart = new Cart();
      }

      if (isPopulatePrice == false)
      {
        cart.CartItemPromoSavings = 0;
        cart.CartTotal = 0;
        cart.ShippingPromoSavings = 0;
        cart.ShippingTotal = 0;
        cart.CartItemTotal = 0;
      }

      if (cart.CartItems != null)
      {
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
      }

      return cart;
    }
  }
}
