using System;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
		[ApiVersion("1.0")]
		[Route("api/v{api-version:apiVersion}/reviews")]
		public class ReviewController : ProxyControllerBase
		{
				public ReviewController(RestClient rest) : base(rest)
				{
				}

				[HttpGet]
				[Auth(Policy = "access_review_api")]
				[SwaggerOperation(Tags = new[] { "review-service" })]
				[Route("{itemId:guid}")]
				public IActionResult Index(Guid itemId)
				{
						return Ok(itemId);
				}
		}
}