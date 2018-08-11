using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Review.v1.UseCases.GetReviews
{
  [ApiVersion("1.0")]
  [Route("api/products")]
  public class Controller : EvtControllerBase
  {
    public Controller(IMediator mediator) : base(mediator) { }

    [HttpGet]
    [Auth(Policy = "access_review_api")]
    [Route("{productId:guid}/reviews")]
    public async Task<IActionResult> Delete(Guid productId)
    {
      return await Eventor.SendStream<GetReviewsRequest, GetReviewsResponse>(
        new GetReviewsRequest {ProductId = productId},
        x => x.Reviews);
    }
  }
}
