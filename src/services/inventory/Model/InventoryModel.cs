using VND.Fw.Infrastructure.AspNetCore;

namespace VND.CoolStore.Services.Inventory.Model
{
  public class InventoryModel : ModelBase
  {
    public string Location { get; set; }
    public int Quantity { get; set; }
    public string Link { get; set; }
  }
}
