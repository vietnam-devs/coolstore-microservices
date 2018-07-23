using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Shared.Catalog.GetProductById
{
  public class GetProductByIdResponse
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public double Price { get; set; }
  }
}
