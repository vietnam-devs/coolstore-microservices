using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Shared.Cart.DeleteItemInCart
{
  public class DeleteItemInCartRequest : RequestIdModelBase
  {
    public Guid ItemId { get; set; }
  }
}
