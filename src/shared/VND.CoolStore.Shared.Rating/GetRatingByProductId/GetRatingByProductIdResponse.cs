using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Shared.Rating.GetRatingByProductId
{
  public class GetRatingByProductIdResponse : IdModelBase
  {
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public double Cost { get; set; }
  }
}
