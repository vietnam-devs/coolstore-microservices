using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Shared.Rating.GetRatings
{
  public class GetRatingsResponse : IdModelBase
  {
    public Guid ProductId { get; set; }
    public double Cost { get; set; }
  }
}
