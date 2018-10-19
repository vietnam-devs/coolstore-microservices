namespace WebUI.Pages.Item
{
  public class NewItemViewModel
  {
    public string ProductName { get; set; }
    public string ImageUrl { get; set; } = "https://picsum.photos/1200/900?image=8";
    public string Description { get; set; }
    public double Price { get; set; }
  }
}
