using System;

namespace VND.CoolStore.Services.Cart.Infrastructure.Dtos
{
  public class ProductDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public double Price { get; set; }
  }
}
