using System;

namespace VND.Services.Inventory.v1.Service.Impl
{
    public class InventoryService : IInventoryService
    {
        public Entity.Inventory GetInventory(Guid itemId)
        {
            return new Entity.Inventory
            {
                ItemId = new Guid("6be1ec2b-3056-42e2-bb1e-2adcf5979a50"),
                Location = "465 Cong Hoa",
                Quantity = 100,
                Link = "http://vietnamdevsgroup.com"
            };
        }
    }
}