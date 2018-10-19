namespace WebUI.Model
{
  public class ConfigModel
  {
    public string CatalogService { get; set; } = "http://api.coolstore.local/catalog";
    public string CartService { get; set; } = "http://api.coolstore.local/cart";
    public string InventoryService { get; set; } = "http://api.coolstore.local/inventory";
    public string RatingService { get; set; } = "http://api.coolstore.local/rating";
  }
}
