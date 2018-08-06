using System;
using MediatR;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public class CheckoutRequest : IRequest<CheckoutResponse>
  {
    public Guid CartId { get; set; }
  }

  public class CheckoutResponse
  {
    public bool IsSucceed { get; set; }
  }
}
