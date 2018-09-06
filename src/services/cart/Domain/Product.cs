using System;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreKit.Domain;
using static NetCoreKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class Product : IdentityBase
  {
    private Product() : base()
    {
    }

    private Product(Guid productId)
      : this(productId, string.Empty, 0.0D, string.Empty)
    {
    }

    private Product(Guid productId, string name, double price, string desc)
      : this(GenerateId(), productId, name, price, desc)
    { 
    }

    private Product(Guid id, Guid productId, string name, double price, string desc)
    {
      Id = id;
      ProductId = productId;
      Name = name;
      Price = price;
      Desc = desc;
    }

    public static Product Load(Guid productId)
    {
      return new Product(productId);
    }

    public static Product Load(Guid productId, string name, double price, string desc)
    {
      return new Product(productId, name, price, desc);
    }

    public Guid ProductId { get; private set; }

    [NotMapped] public string Name { get; private set; }

    [NotMapped] public double Price { get; private set; }

    [NotMapped] public string Desc { get; private set; }

    public CartItem CartItem { get; private set; }

    public Product LinkCartItem(CartItem cartItem)
    {
      CartItem = cartItem;
      return this;
    }
  }
}
