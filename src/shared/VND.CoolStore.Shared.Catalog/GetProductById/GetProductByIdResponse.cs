using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Shared.Catalog.GetProductById
{
  public class GetProductByIdResponse : IdModelBase
  {
    public string Name { get; set; }
    public string Desc { get; set; }
    public double Price { get; set; }
  }
}
