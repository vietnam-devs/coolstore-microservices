using System.Collections.Generic;

namespace WebUI.Model
{
  public class Pagination<TModel>
    where TModel : class 
  {
    public IEnumerable<TModel> Items { get; set; } = new List<TModel>();
    public int TotalItems { get; set; } = 6;
    public int PageSize { get; set; } = 6;
    public int CurrentPage { get; set; } = 1;
  }
}
