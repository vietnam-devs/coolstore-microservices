using VND.CoolStore.Services.Review.Dtos;

namespace VND.CoolStore.Services.Review.Extensions
{
  public static class ReviewExtensions
  {
    public static ReviewDto ToDto(this Domain.Review review)
    {
      return new ReviewDto
      {
        Id = review.Id,
        Content = review.Content,
        AuthorId = review.ReviewAuthor.Id,
        AuthorName = review.ReviewAuthor.UserName
      };
    }
  }
}
