using System.Reactive.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Review.v1.UseCases.AddReview
{
  [ApiVersion("1.0")]
  [Route("api/reviews")]
  public class Controller : EvtControllerBase
  {
    public Controller(IMediator mediator) : base(mediator) { }

    [HttpPost]
    [Auth(Policy = "access_review_api")]
    public async Task<IActionResult> Post(AddReviewRequest request) =>
      await Eventor.SendStream<AddReviewRequest, AddReviewResponse>(request, x => x.Result);
  }
}
