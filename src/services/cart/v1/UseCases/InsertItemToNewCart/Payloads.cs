using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using VND.CoolStore.Services.Cart.Domain.Dtos;

namespace VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart
{
  public class InsertItemToNewCartRequest : IRequest<InsertItemToNewCartResponse>
  {
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
  }

  public class InsertItemToNewCartResponse
  {
    public CartDto Result { get; set; }
  }
}
