using VND.CoolStore.Services.Inventory.Dtos;

namespace VND.CoolStore.Services.Inventory.Extensions
{
  public static class InventoryExtensions
  {
    public static InventoryDto ToDto(this Domain.Inventory inv)
    {
      return new InventoryDto
      {
        Link = inv.Link,
        Location =  inv.Location,
        Quantity = inv.Quantity
      };
    }
  }
}
