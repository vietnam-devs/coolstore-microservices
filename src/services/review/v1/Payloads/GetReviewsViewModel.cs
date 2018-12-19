using System.Collections.Generic;
using VND.CoolStore.Services.Review.Dtos;

namespace VND.CoolStore.Services.Review.v1.Payloads
{
  public class GetReviewsViewModel
  {
    public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
  }
}
