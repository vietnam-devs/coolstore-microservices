using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using VND.CoolStore.Services.Review.Dtos;

namespace VND.CoolStore.Services.Review.v1.UseCases.UpdateReview
{
  public class UpdateReviewRequest : IRequest<UpdateReviewResponse>
  {
    [Required]
    public Guid ReviewId { get; set; }

    [Required]
    public string Content { get; set; }
  }

  public class UpdateReviewResponse
  {
    public ReviewDto Result { get; set; }
  }
}
