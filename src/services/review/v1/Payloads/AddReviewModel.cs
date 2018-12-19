using System;
using System.ComponentModel.DataAnnotations;

namespace VND.CoolStore.Services.Review.v1.Payloads
{
  public class AddReviewModel
  {
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public string Content { get; set; }
  }
}
