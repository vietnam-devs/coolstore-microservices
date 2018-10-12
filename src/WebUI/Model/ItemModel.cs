using System;
using System.Collections.Generic;

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

  public class Pagination<TModel>
    where TModel : class 
  {
    public IEnumerable<TModel> Items { get; set; } = new List<TModel>();
    public int TotalItems { get; set; } = 6;
    public int PageSize { get; set; } = 6;
    public int CurrentPage { get; set; } = 1;
  }
}
