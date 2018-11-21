using System;
using NetCoreKit.Domain;

namespace VND.CoolStore.Services.Inventory.v1.Domain
{
  public class Inventory : AggregateRootBase
  {
    internal Inventory() : base()
    {
    }

    public Inventory(Guid id) : base(id)
    {
    }

    public string Location { get; set; }
    public int Quantity { get; set; }
    public string Link { get; set; }
  }
}
