using System;

namespace VND.CoolStore.Services.Inventory.UseCases.Service.Impl
{
    public class InventoryService : IInventoryService
    {
        public Domain.Inventory GetInventory(Guid itemId)
        {
            return new Domain.Inventory
            {
                ItemId = new Guid("6be1ec2b-3056-42e2-bb1e-2adcf5979a50"),
                Location = "465 Cong Hoa",
                Quantity = 100,
                Link = "http://vietnamdevsgroup.com"
            };
        }
    }
}