using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using VND.CoolStore.Services.Cart.Dtos;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  public class GetCartRequest : IRequest<GetCartResponse>
  {
    [Required]
    public Guid CartId { get; set; }
  }

  public class GetCartResponse
  {
    public CartDto Result { get; set; }
  }
}
