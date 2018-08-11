using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Review.v1.UseCases.RemoveReview
{
  [ApiVersion("1.0")]
  [Route("api/reviews")]
  public class Controller : EvtControllerBase
  {
    public Controller(IMediator mediator) : base(mediator) { }

    [HttpDelete]
    [Auth(Policy = "access_review_api")]
    [Route("{reviewId:guid}")]
    public async Task<IActionResult> Delete(Guid reviewId)
    {
      return await Eventor.SendStream<RemoveReviewRequest, RemoveReviewResponse>(
        new RemoveReviewRequest {ReviewId = reviewId},
        x => x.ReviewId);
    }
  }
}
