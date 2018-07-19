using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Extensions;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.FW.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.ApiGateway.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/inventories")]
  public class InventoryController : ProxyControllerBase
  {
    private readonly string _inventoryServiceUri;

    public InventoryController(RestClient restClient, IConfiguration config, IHostingEnvironment env) : base(restClient)
    {
      _inventoryServiceUri = config.GetHostUri(env, "Inventory");
    }

    [HttpGet]
    [Auth(Policy = "access_inventory_api")]
    [SwaggerOperation(Tags = new[] { "inventory-service" })]
    [Route("availability/{itemId:guid}")]
    public async Task<ActionResult<InventoryModel>> Availability(Guid itemId)
    {
      InitRestClientWithOpenTracing();

      string getAvailabilityEndPoint = $"{_inventoryServiceUri}/api/v1/availability/{itemId}";
      return await RestClient.GetAsync<InventoryModel>(getAvailabilityEndPoint);
    }
  }
}
