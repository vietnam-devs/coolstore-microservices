using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public class DeleteItemRequest : IRequest<DeleteItemResponse>
  {
    [Required]
    public Guid CartId { get; set; }
    [Required]
    public Guid ProductId { get; set; }
  }

  public class DeleteItemResponse
  {
    public Guid ProductId { get; set; }
  }
}
