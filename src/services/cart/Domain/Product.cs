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

    public Product(Guid id) : this(id, string.Empty, 0.0D, string.Empty)
    { 
    }

    public Product(Guid id, string name, double price, string desc)
    {
      Id = id;
      Name = name;
      Price = price;
      Desc = desc;
    }

    [NotMapped]
    public string Name { get; private set; }

    [NotMapped]
    public double Price { get; private set; }

    [NotMapped]
    public string Desc { get; private set; }
  }
}
