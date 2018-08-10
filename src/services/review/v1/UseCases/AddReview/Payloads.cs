using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using VND.CoolStore.Services.Review.Dtos;

namespace VND.CoolStore.Services.Review.v1.UseCases.AddReview
{
  public class AddReviewRequest : IRequest<AddReviewResponse>
  {
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public string Content { get; set; }
  }

  public class AddReviewResponse
  {
    public ReviewDto Result { get; set; }
  }
}
