using System;
using System.ComponentModel.DataAnnotations;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Shared.Cart.InsertItemToNewCart
{
  public class InsertItemToNewCartRequest : ModelBase
		{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }
  }
}
