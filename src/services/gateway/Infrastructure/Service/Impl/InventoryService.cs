using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Model;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.ApiGateway.Infrastructure.Service.Impl
{
  public class InventoryService : ProxyServiceBase, IInventoryService
  {
    private readonly string _inventoryServiceUri;

    public InventoryService(
      RestClient rest,
      IConfiguration config,
      IHostingEnvironment env) : base(rest)
    {
      _inventoryServiceUri = config.GetHostUri(env, "Inventory");
    }

    public async Task<InventoryModel> GetAvailabilityAsync(Guid itemId)
    {
      string getAvailabilityEndPoint = $"{_inventoryServiceUri}/api/v1/availability/{itemId}";
      return await RestClient.GetAsync<InventoryModel>(getAvailabilityEndPoint);
    }
  }
}
