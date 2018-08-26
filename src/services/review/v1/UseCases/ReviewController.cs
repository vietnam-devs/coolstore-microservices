using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore.Authz;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using VND.CoolStore.Services.Review.v1.UseCases.AddReview;
using VND.CoolStore.Services.Review.v1.UseCases.GetReviews;
using VND.CoolStore.Services.Review.v1.UseCases.RemoveReview;
using VND.CoolStore.Services.Review.v1.UseCases.UpdateReview;

namespace VND.CoolStore.Services.Review.v1.UseCases
{
  [ApiVersion("1.0")]
  [Route("api/reviews")]
  public class ReviewController : Controller
  {
    [HttpGet]
    [Auth(Policy = "access_review_api")]
    [Route("{productId:guid}/reviews")]
    public async Task<IActionResult> Get([FromServices] IMediator eventor, Guid productId, CancellationToken cancellationToken)
    {
      return await eventor.SendStream<GetReviewsRequest, GetReviewsResponse>(
        new GetReviewsRequest { ProductId = productId },
        x => x.Reviews,
        cancellationToken);
    }

    [HttpPost]
    [Auth(Policy = "access_review_api")]
    public async Task<IActionResult> Post([FromServices] IMediator eventor, AddReviewRequest request, CancellationToken cancellationToken) =>
      await eventor.SendStream<AddReviewRequest, AddReviewResponse>(
        request,
        x => x.Result,
        cancellationToken);

    [HttpDelete]
    [Auth(Policy = "access_review_api")]
    [Route("{reviewId:guid}")]
    public async Task<IActionResult> Delete([FromServices] IMediator eventor, Guid reviewId, CancellationToken cancellationToken)
    {
      return await eventor.SendStream<RemoveReviewRequest, RemoveReviewResponse>(
        new RemoveReviewRequest { ReviewId = reviewId },
        x => x.ReviewId,
        cancellationToken);
    }

    [HttpPut]
    [Auth(Policy = "access_review_api")]
    [Route("{reviewId:guid}/{content}")]
    public async Task<IActionResult> Put([FromServices] IMediator eventor, Guid reviewId, string content, CancellationToken cancellationToken)
    {
      return await eventor.SendStream<UpdateReviewRequest, UpdateReviewResponse>(
        new UpdateReviewRequest
        {
          ReviewId = reviewId,
          Content = content
        },
        x => x.Result,
        cancellationToken);
    }
  }
}
