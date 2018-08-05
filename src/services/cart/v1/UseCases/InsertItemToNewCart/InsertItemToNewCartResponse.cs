using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  public class InsertItemToNewCartResponse : ModelBase
  {
    public Guid Id { get; set; }
  }
}
