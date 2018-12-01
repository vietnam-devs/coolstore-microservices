namespace VND.CoolStore.Services.Inventory.v1.Dtos
{
  public class InventoryDto
  {
    public string Location { get; set; }
    public int Quantity { get; set; }
    public string Link { get; set; }
  }

  public static class InventoryExtensions
  {
    public static InventoryDto ToDto(this Domain.Inventory inv)
    {
      return new InventoryDto
      {
        Link = inv.Link,
        Location = inv.Location,
        Quantity = inv.Quantity
      };
    }
  }
}
