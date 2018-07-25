using System;
using System.ComponentModel.DataAnnotations.Schema;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class Product : IdentityBase
  {
    internal Product() : base()
    {
    }

    public Product(Guid productId)
      : this(productId, string.Empty, 0.0D, string.Empty)
    {
    }

    public Product(Guid productId, string name, double price, string desc)
      : this(Guid.NewGuid(), productId, name, price, desc)
    { 
    }

    public Product(Guid id, Guid productId, string name, double price, string desc)
    {
      Id = id;
      ProductId = productId;
      Name = name;
      Price = price;
      Desc = desc;
    }

    public Guid ProductId { get; private set; }

    [NotMapped]
    public string Name { get; private set; }

    [NotMapped]
    public double Price { get; private set; }

    [NotMapped]
    public string Desc { get; private set; }
  }
}
