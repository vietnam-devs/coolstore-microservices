using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Rating.v1.Grpc;
using static VND.CoolStore.Services.Rating.v1.Grpc.RatingService;

namespace VND.CoolStore.Services.WebAggregator.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/ratings")]
    public class RatingController : ControllerBase
    {
        private readonly RatingServiceClient _serviceClient;

        public RatingController(RatingServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _serviceClient.GetRatingsAsync(new Empty());
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            var result = await _serviceClient.GetRatingByProductIdAsync(
                new GetRatingByProductIdRequest {ProductId = productId.ToString()});
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRatingRequest request)
        {
            var result = await _serviceClient.CreateRatingAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRatingRequest request)
        {
            var result = await _serviceClient.UpdateRatingAsync(request);
            return Ok(result);
        }
    }
}
