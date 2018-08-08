using System;
using System.Collections.Generic;

namespace VND.CoolStore.Services.Cart.Dtos
{
  public class CartDto
  {
    public Guid Id { get; set; }
    public double CartItemTotal { get; set; }
    public double CartItemPromoSavings { get; set; }
    public double ShippingTotal { get; set; }
    public double ShippingPromoSavings { get; set; }
    public double CartTotal { get; set; }
    public bool IsCheckout { get; set; }
    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();

    public class CartItemDto
    {
      public int Quantity { get; set; }
      public double Price { get; set; }
      public double PromoSavings { get; set; }
      public Guid ProductId { get; set; }
      public string ProductName { get; set; }
    }
  }
}
