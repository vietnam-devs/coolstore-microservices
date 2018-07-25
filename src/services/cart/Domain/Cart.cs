using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
}
