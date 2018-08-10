using System;

namespace VND.CoolStore.Services.Review.Dtos
{
  public partial class ReviewDto
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
  }
}
