using VND.CoolStore.Services.Review.v1.Grpc;

namespace VND.CoolStore.Services.Review.Extensions
{
    public static class ReviewExtensions
    {
        public static ReviewDto ToDto(this Domain.Review review)
        {
            return new ReviewDto
            {
                Id = review.Id.ToString(),
                Content = review.Content,
                AuthorId = review.ReviewAuthor.Id.ToString(),
                AuthorName = review.ReviewAuthor.UserName,
                ProductId = review.ReviewProduct.Id.ToString(),
                ProductName = review.ReviewProduct.Name
            };
        }
    }
}
