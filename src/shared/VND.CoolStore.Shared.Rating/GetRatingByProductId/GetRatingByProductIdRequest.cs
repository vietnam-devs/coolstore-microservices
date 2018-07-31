using System;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Shared.Rating.GetRatingByProductId
{
  public class GetRatingByProductIdRequest : RequestIdModelBase
  {
    public Guid ProductId { get; set; }
  }
}
