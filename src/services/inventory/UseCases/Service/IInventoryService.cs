using System;

namespace VND.Services.Inventory.UseCases.Service
{
    public interface IInventoryService
    {
        Entity.Inventory GetInventory(Guid itemId);
    }
}
