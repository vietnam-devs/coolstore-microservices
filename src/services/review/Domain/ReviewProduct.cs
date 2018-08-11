using System;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreKit.Domain;

namespace VND.CoolStore.Services.Review.Domain
{
  public class ReviewProduct : IdentityBase
  {
    internal ReviewProduct(Guid id)
      : this(id, string.Empty, 0.0D, string.Empty)
    {
    }

    public ReviewProduct(string name, double price, string desc)
      : this(Guid.NewGuid(), name, price, desc)
    { 
    }

    public ReviewProduct(Guid id, string name, double price, string desc)
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
