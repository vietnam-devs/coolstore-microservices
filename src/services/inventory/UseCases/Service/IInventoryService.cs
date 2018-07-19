using System;

namespace VND.CoolStore.Services.Inventory.UseCases.Service
{
  public interface IInventoryService
  {
    Domain.Inventory GetInventory(Guid itemId);
  }
}
