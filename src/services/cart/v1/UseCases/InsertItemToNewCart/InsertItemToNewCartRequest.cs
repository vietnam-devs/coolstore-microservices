using System;
using System.ComponentModel.DataAnnotations;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  public class InsertItemToNewCartRequest : RequestModelBase
  {
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }
  }
}
