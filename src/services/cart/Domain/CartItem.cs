using System;
using System.ComponentModel.DataAnnotations;
using NetCoreKit.Domain;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class CartItem : EntityBase
  {
    internal CartItem() : base(Guid.NewGuid())
    {
    }

    public CartItem(Guid id) : base(id)
    {
    }

    [Required]
    public int Quantity { get; set; }

    public double Price { get; set; }

    [Required]
    public double PromoSavings { get; set; }

    [Required]
    public Product Product { get; set; }
  }
}
