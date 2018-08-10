using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;
using VND.CoolStore.Services.Review.Dtos;

namespace VND.CoolStore.Services.Review.v1.UseCases.GetReviews
{
  public class GetReviewsRequest : IRequest<GetReviewsResponse>
  {
    [Required]
    public Guid ProductId { get; set; }
  }

  public class GetReviewsResponse
  {
    public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
  }
}
