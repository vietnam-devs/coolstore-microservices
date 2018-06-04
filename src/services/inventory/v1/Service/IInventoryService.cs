using System;

namespace VND.Services.Inventory.v1.Service
{
    public interface IInventoryService
    {
        Entity.Inventory GetInventory(Guid itemId);
    }
}