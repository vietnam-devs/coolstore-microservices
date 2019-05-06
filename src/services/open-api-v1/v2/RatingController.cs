using System;
using System.Threading.Tasks;
using Coolstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VND.CoolStore.Services.OpenApiV1.v1.Grpc;
using static Coolstore.RatingService;

namespace VND.CoolStore.Services.OpenApiV1.v2
{
    [ApiVersion("2.0")]
    [Route("rating/api/ratings")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly AppOptions _appOptions;
        private readonly RatingServiceClient _ratingServiceClient;

        public RatingController(IOptions<AppOptions> options, RatingServiceClient ratingServiceClient)
        {
            _appOptions = options.Value;
            _ratingServiceClient = ratingServiceClient;
        }

        [HttpGet("ping")]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetRatings()
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "rating-service",
                async headers =>
                {
                    var response = await _ratingServiceClient.GetRatingsAsync(
                        new Google.Protobuf.WellKnownTypes.Empty(),
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.Ratings);
                });
        }

        [HttpGet("productId:guid")]
        public async ValueTask<IActionResult> GetRatingByProductId(Guid productId)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "rating-service",
                async headers =>
                {
                    var request = new GetRatingByProductIdRequest
                    {
                        ProductId = productId.ToString()
                    };

                    var response = await _ratingServiceClient.GetRatingByProductIdAsync(
                        request,
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.Rating);
                });
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateRating(CreateRatingRequest request)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "rating-service",
                async headers =>
                {
                    var response = await _ratingServiceClient.CreateRatingAsync(
                        request,
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.Rating);
                });
        }

        [HttpPut]
        public async ValueTask<IActionResult> UpdateRating(UpdateRatingRequest request)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "rating-service",
                async headers =>
                {
                    var response = await _ratingServiceClient.UpdateRatingAsync(
                        request,
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.Rating);
                });
        }
    }
}
