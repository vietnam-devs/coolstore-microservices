using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Service;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.CoolStore.Shared.Rating.GetRatingByProductId;
using VND.CoolStore.Shared.Rating.GetRatings;
using VND.Fw.Domain;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/ratings")]
    public class RatingController : FW.Infrastructure.AspNetCore.ControllerBase
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IRatingService _ratingService;
        private readonly ILogger<ProductController> _logger;

        public RatingController(
              IRatingService ratingService,
              IUrlHelper urlHelper,
              ILoggerFactory logger)
        {
            _urlHelper = urlHelper;
            _ratingService = ratingService;
            _logger = logger.CreateLogger<ProductController>();
        }

        [HttpGet]
        [Auth(Policy = "access_rating_api")]
        [SwaggerOperation(Tags = new[] { "rating-service" })]
        [Route("{itemId:guid}")]
        public IActionResult Index(Guid itemId)
        {
            return Ok(itemId);
        }

        [HttpGet(Name = nameof(GetAllRatings))]
        [Auth(Policy = "access_rating_api")]
        [SwaggerOperation(Tags = new[] { "ratting-service" })]
        public async Task<ActionResult<IEnumerable<RatingModel>>> GetAllRatings([FromQuery] Criterion criterion)
        {
            _logger.LogDebug($"VND: {HttpContext.Request.Headers.ToArray().ToString()}");

            IEnumerable<GetRatingsResponse> ratings =
              await _ratingService.GetRatingsAsync(
                new GetRatingsRequest { Headers = HttpContext.Request.GetOpenTracingInfo() });

            int numberOfRatings = ratings.Count();

            Response.AddPaginateInfo(criterion, numberOfRatings);

            IEnumerable<dynamic> toReturn = ratings.Select(x => _urlHelper.ExpandSingleItem(nameof(GetRatingByProductId), x));

            return Ok(new
            {
                value = toReturn,
            });
        }

        [HttpGet]
        [Auth(Policy = "access_rating_api")]
        [SwaggerOperation(Tags = new[] { "ratting-service" })]
        [Route("{id:guid}", Name = nameof(GetRatingByProductId))]
        public async Task<ActionResult<GetRatingByProductIdResponse>> GetRatingByProductId(Guid productId)
        {
            _logger.LogDebug($"VND: {HttpContext.Request.Headers.ToArray().ToString()}");

            GetRatingByProductIdResponse rating = await _ratingService.GetRatingByProductIdAsync(new GetRatingByProductIdRequest { ProductId = productId });
            return Ok(rating);
        }
    }
}
