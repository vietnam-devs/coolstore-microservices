using System;
using System.ComponentModel.DataAnnotations;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public class UpdateItemInCartRequest : RequestModelBase
  {
    [Required]
    public Guid CartId { get; set; }

    //[Required]
    //public Guid ItemId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }
  }
}
