using System;
using MediatR;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public class DeleteItemRequest : IRequest<DeleteItemResponse>
  {
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
  }

  public class DeleteItemResponse
  {
    public Guid ProductId { get; set; }
  }
}
