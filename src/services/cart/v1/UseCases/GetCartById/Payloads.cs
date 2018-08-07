using System;
using MediatR;
using VND.CoolStore.Services.Cart.Domain.Dtos;

namespace VND.CoolStore.Services.Cart.v1.UseCases.GetCartById
{
  public class GetCartRequest : IRequest<GetCartResponse>
  {
    public Guid CartId { get; set; }
  }

  public class GetCartResponse
  {
    public CartDto Result { get; set; }
  }
}
