using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore.Authz;
using VND.CoolStore.Services.Review.v1.Grpc;
using static VND.CoolStore.Services.Review.v1.Grpc.ReviewService;
using static VND.CoolStore.Services.Review.v1.Grpc.PingService;

namespace VND.CoolStore.Services.WebAggregator.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewServiceClient _reviewServiceClient;
        private readonly PingServiceClient _pingServiceClient;

        public ReviewController(ReviewServiceClient reviewServiceClient, PingServiceClient pingServiceClient)
        {
            _reviewServiceClient = reviewServiceClient;
            _pingServiceClient = pingServiceClient;
        }

        [HttpGet]
        //[Auth(Policy = "access_review_api")]
        [Route("{productId:guid}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            return Ok(
                await _reviewServiceClient.GetReviewsAsync(new GetReviewsRequest {ProductId = productId.ToString()})
            );
        }

        [HttpGet]
        // [Auth(Policy = "access_review_api")]
        [Route("ping")]
        public async Task<IActionResult> GetPing()
        {
            return Ok(await _pingServiceClient.PingAsync(new Empty()));
        }

        [HttpPost]
        //[Auth(Policy = "access_review_api")]
        public async Task<IActionResult> Post(CreateReviewRequest request)
        {
            return Ok(await _reviewServiceClient.CreateReviewAsync(request));
        }

        [HttpDelete]
        //[Auth(Policy = "access_review_api")]
        [Route("{reviewId:guid}")]
        public async Task<IActionResult> Delete(Guid reviewId)
        {
            return Ok(
                await _reviewServiceClient.DeleteReviewAsync(new DeleteReviewRequest {ReviewId = reviewId.ToString()})
            );
        }

        [HttpPut]
        //[Auth(Policy = "access_review_api")]
        [Route("{reviewId:guid}/{content}")]
        public async Task<IActionResult> Put(Guid reviewId, string content)
        {
            return Ok(
                await _reviewServiceClient.EditReviewAsync(new EditReviewRequest
                {
                    ReviewId = reviewId.ToString(),
                    Content = content
                })
            );
        }
    }
}
