using System;

namespace VND.CoolStore.Services.ApiGateway.Model
{
  public class RatingModel
  {
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public double Cost { get; set; }
  }
}
