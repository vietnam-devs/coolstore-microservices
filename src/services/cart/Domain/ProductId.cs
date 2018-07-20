using System;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class ProductId : IdentityBase
  {
    private ProductId() { }
    public ProductId(Guid id) : base(id)
    {
    }
  }
}
