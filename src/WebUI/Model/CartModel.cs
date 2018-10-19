using System;
using System.Collections.Generic;

namespace WebUI.Model
{
  public class CartModel
  {
    public Guid CartId { get; set; }
    // public Guid ProductId { get; set; }
    // public int Quantity { get; set; } = 1;
    public double ShippingTotal { get; set; } = 0D;
    public double CartItemPromoSavings { get; set; } = 0D;
    public double CartItemTotal { get; set; } = 0D;
    public double ShippingPromoSavings { get; set; } = 0D;
    public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();
  }

  public class CartItemModel
  {
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; } = 0D;
    public int Quantity { get; set; } = 1;
    public string ImageUrl { get; set; } = "https://picsum.photos/120/75?image=8";
  }
}
