using System;
using System.Threading.Tasks;
using VND.CoolStore.Services.ApiGateway.Model;

namespace VND.CoolStore.Services.ApiGateway.Infrastructure.Service
{
  public interface IInventoryService
  {
    Task<InventoryModel> GetAvailabilityAsync(Guid itemId);
  }
}
