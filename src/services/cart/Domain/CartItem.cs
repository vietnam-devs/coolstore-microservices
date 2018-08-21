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

    [Required] public int Quantity { get; set; }

    public double Price { get; set; }

    [Required] public double PromoSavings { get; set; }

    public Cart Cart { get; private set; }
    public Guid CartId { get; private set; }

    public Product Product { get; set; }

    public CartItem LinkCart(Cart cart)
    {
      Cart = cart;
      CartId = cart.Id;
      return this;
    }

    public CartItem LinkProduct(Product product)
    {
      product.LinkCartItem(this);
      Product = product;
      return this;
    }
  }
}
