using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace VND.CoolStore.Services.Cart.v1.UseCases.Checkout
{
  public class CheckoutRequest : IRequest<CheckoutResponse>
  {
    [Required]
    public Guid CartId { get; set; }
  }

  public class CheckoutResponse
  {
    public bool IsSucceed { get; set; }
  }
}
