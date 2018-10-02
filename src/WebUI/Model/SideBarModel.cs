namespace WebUI.Model
{
  public class SideBarModel
  {
    public int Min { get; set; } = 1;
    public int Max { get; set; } = 100;
    public string PriceRange { get; set; } = "300";
    public bool Sale { get; set; } = false;
  }
}
