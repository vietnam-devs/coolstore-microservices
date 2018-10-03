using System;

namespace WebUI.Model
{
  public class CartModel
  {
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public double CartTotal { get; set; } = 0D;
    public double CartItemPromoSavings { get; set; } = 0D;
    public double ShippingTotal { get; set; } = 0D;
    public double ShippingPromoSavings { get; set; } = 0D;
  }
}
