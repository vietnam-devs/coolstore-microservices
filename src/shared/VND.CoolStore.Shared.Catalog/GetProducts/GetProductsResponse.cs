using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Shared.Catalog.GetProducts
{
  public class GetProductsResponse : IdModelBase
  {
    public string Name { get; set; }
    public string Desc { get; set; }
    public double Price { get; set; }
  }
}
