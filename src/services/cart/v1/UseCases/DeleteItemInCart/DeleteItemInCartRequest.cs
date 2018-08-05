using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public class DeleteItemInCartRequest : RequestIdModelBase
  {
    public Guid ProductId { get; set; }
  }
}
