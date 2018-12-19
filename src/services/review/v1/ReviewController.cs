using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.Authz;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Review.Extensions;
using VND.CoolStore.Services.Review.v1.Payloads;

namespace VND.CoolStore.Services.Review.v1
{
  [ApiVersion("1.0")]
  [Route("api/reviews")]
  public class ReviewController : Controller
  {
    private readonly IQueryRepositoryFactory _queryRepositoryFactory;
    private readonly IRepositoryAsyncFactory _repositoryFactory;

    public ReviewController(IQueryRepositoryFactory queryRepositoryFactory, IRepositoryAsyncFactory repositoryFactory)
    {
      _queryRepositoryFactory = queryRepositoryFactory;
      _repositoryFactory = repositoryFactory;
    }

    [HttpGet]
    [Auth(Policy = "access_review_api")]
    [Route("{productId:guid}/reviews")]
    public async Task<IActionResult> Get(Guid productId)
    {
      var reviewQueryRepo = _queryRepositoryFactory.QueryEfRepository<Domain.Review>();

      var reviews = await reviewQueryRepo
        .ListAsync(
          queryable => queryable
            .Include(x => x.ReviewAuthor)
            .Include(x => x.ReviewProduct));

      var result = reviews.Where(x => x.ReviewProduct?.Id == productId);

      if (reviews == null)
        throw new Exception($"Couldn't find the review with product#{productId}.");

      return Ok(new GetReviewsViewModel
      {
        Reviews = result
          .Select(x => x.ToDto())
          .ToList()
      });
    }

    [HttpPost]
    [Auth(Policy = "access_review_api")]
    public async Task<IActionResult> Post(AddReviewModel request)
    {
      var reviewRepository = _repositoryFactory.RepositoryAsync<Domain.Review>();

      var review = Domain.Review
        .Load(request.Content)
        .AddAuthor(request.UserId)
        .AddProduct(request.ProductId);

      var result = await reviewRepository.AddAsync(review);
      return Ok(new AddReviewViewModel { Result = result.ToDto() });
    }

    [HttpDelete]
    [Auth(Policy = "access_review_api")]
    [Route("{reviewId:guid}")]
    public async Task<IActionResult> Delete(Guid reviewId)
    {
      var reviewQueryRepo = _queryRepositoryFactory.QueryEfRepository<Domain.Review>();
      var reviewRepo = _repositoryFactory.RepositoryAsync<Domain.Review>();

      var review = await reviewQueryRepo.FindOneAsync(x => x.Id == reviewId);
      if (review == null)
        throw new Exception($"Couldn't find the review #{reviewId}.");

      var result = await reviewRepo.DeleteAsync(review);
      return Ok(result.Id);
    }

    [HttpPut]
    [Auth(Policy = "access_review_api")]
    [Route("{reviewId:guid}/{content}")]
    public async Task<IActionResult> Put(Guid reviewId, string content)
    {
      var reviewQueryRepo = _queryRepositoryFactory.QueryEfRepository<Domain.Review>();
      var reviewRepo = _repositoryFactory.RepositoryAsync<Domain.Review>();

      var review = await reviewQueryRepo.FindOneAsync(x => x.Id == reviewId);
      if (review == null)
        throw new Exception($"Couldn't find the review #{reviewId}.");

      review.Content = content;
      var result = await reviewRepo.UpdateAsync(review);

      return Ok(new UpdateReviewViewModel {Result = result.ToDto()});
    }
  }
}
