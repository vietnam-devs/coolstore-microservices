using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
		[ApiVersion("1.0")]
		[Route("api/v{api-version:apiVersion}/inventories")]
		public class InventoryController : FW.Infrastructure.AspNetCore.ControllerBase
		{
				private readonly string _inventoryServiceUri;

				public InventoryController(RestClient restClient, IHostingEnvironment env) : base(restClient)
				{
						_inventoryServiceUri = env.IsDevelopment()
								? "http://localhost:5004"
								: $"http://{Environment.GetEnvironmentVariable("INVENTORY_SERVICE_SERVICE_HOST")}:{Environment.GetEnvironmentVariable("INVENTORY_SERVICE_SERVICE_PORT")}";
				}

				[HttpGet]
				[Auth(Policy = "access_inventory_api")]
				[SwaggerOperation(Tags = new[] { "inventory-service" })]
				[Route("availability/{itemId:guid}")]
				public async Task<ActionResult<InventoryModel>> Availability(Guid itemId)
        {
						InitRestClientWithOpenTracing();

						var getAvailabilityEndPoint = $"{_inventoryServiceUri}/api/v1/availability/{itemId}";
						return await RestClient.GetAsync<InventoryModel>(getAvailabilityEndPoint);
        }
    }
}