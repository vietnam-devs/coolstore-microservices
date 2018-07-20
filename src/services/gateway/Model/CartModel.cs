using System;
using System.Collections.Generic;

namespace VND.CoolStore.Services.ApiGateway.Model
{
  public class CartModel
  {
    public Guid Id { get; set; }
    public double CartItemTotal { get; set; }
    public double CartItemPromoSavings { get; set; }
    public double ShippingTotal { get; set; }
    public double ShippingPromoSavings { get; set; }
    public double CartTotal { get; set; }
    public ICollection<CartItemModel> CartItems { get; set; }
  }
}
