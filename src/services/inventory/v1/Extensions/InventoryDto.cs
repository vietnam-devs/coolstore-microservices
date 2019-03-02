using VND.CoolStore.Services.Inventory.v1.Grpc;

namespace VND.CoolStore.Services.Inventory.v1.Extensions
{
    public static class InventoryExtensions
    {
        public static InventoryDto ToDto(this Domain.Inventory inv)
        {
            return new InventoryDto
            {
                Id = inv.Id.ToString(),
                Link = inv.Link,
                Location = inv.Location,
                Quantity = inv.Quantity
            };
        }
    }
}
