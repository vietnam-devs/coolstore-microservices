using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class Cart : EntityBase
  {
    internal Cart() { }

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
    public ICollection<CartItem> CartItems { get; set; }
  }
}
