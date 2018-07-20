using System.ComponentModel.DataAnnotations;
using VND.CoolStore.Services.Cart.Domain;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class CartItem : EntityBase
  {
    internal CartItem() { }

    [Required]
    public double Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public double PromoSavings { get; set; }

    [Required]
    public ProductId ProductId { get; set; }
  }
}
