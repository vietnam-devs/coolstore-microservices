using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace VND.CoolStore.Services.Review.v1.UseCases.RemoveReview
{
  public class RemoveReviewRequest : IRequest<RemoveReviewResponse>
  {
    [Required]
    public Guid ReviewId { get; set; }
  }

  public class RemoveReviewResponse
  {
    public Guid ReviewId { get; set; }
  }
}
