using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Review.v1.UseCases.UpdateReview
{
  [ApiVersion("1.0")]
  [Route("api/reviews")]
  public class Controller : EvtControllerBase
  {
    public Controller(IMediator mediator) : base(mediator) { }

    [HttpPut]
    [Auth(Policy = "access_review_api")]
    [Route("{reviewId:guid}/{content}")]
    public async Task<IActionResult> Put(Guid reviewId, string content)
    {
      return await Eventor.SendStream<UpdateReviewRequest, UpdateReviewResponse>(
        new UpdateReviewRequest
        {
          ReviewId = reviewId,
          Content = content
        },
        x => x.Result);
    }
  }
}
