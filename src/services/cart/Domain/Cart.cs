using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NetCoreKit.Domain;

namespace VND.CoolStore.Services.Cart.Domain
{
  public sealed class Cart : EntityBase
  {
    internal Cart() : base(Guid.NewGuid())
    {
    }

    public Cart(Guid id) : base(id)
    {
    }

    public static Cart Load()
    {
      return new Cart();
    }

    public static Cart Load(Guid id)
    {
      return new Cart(id);
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
      CartItems = CartItems.Where(y => y.Id != itemId).ToList();
      return this;
    }
  }
}
