using System;

namespace WebUI.Model
{
  public class ItemModel
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Desc { get; set; }
    public string ImageUrl { get; set; } = "https://picsum.photos/1200/900?image=8";
  }
}
