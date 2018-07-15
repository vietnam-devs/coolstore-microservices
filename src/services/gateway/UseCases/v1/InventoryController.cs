using System;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
		[ApiVersion("1.0")]
		[Route("api/v{api-version:apiVersion}/inventories")]
		public class InventoryController : FW.Infrastructure.AspNetCore.ControllerBase
		{
				public InventoryController(RestClient rest) : base(rest)
				{
				}

				[HttpGet]
				[Auth(Policy = "access_inventory_api")]
				[SwaggerOperation(Tags = new[] { "inventory-service" })]
				[Route("availability/{itemId:guid}")]
				public IActionResult Availability(Guid itemId)
        {
						return Ok(itemId);
        }
    }
}