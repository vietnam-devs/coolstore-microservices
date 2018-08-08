using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using VND.CoolStore.Services.Cart.Dtos;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public class UpdateItemInCartRequest : IRequest<UpdateItemInCartResponse>
  {
    [Required]
    public Guid CartId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }
  }

  public class UpdateItemInCartResponse
  {
    public CartDto Result { get; set; }
  }
}
