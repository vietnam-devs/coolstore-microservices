using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Model;

namespace WebUI.Services
{
  public class ItemService
  {
    private readonly IEnumerable<ItemModel> _results;

    public ItemService()
    {
      _results = new List<ItemModel>();
      for (var i = 0; i < 100; i++)
      {
        _results = _results.Append(new ItemModel
        {
          Id = Guid.NewGuid(),
          Name = $"Item {i + 1}",
          Price = 10.5D * (i + 1),
          ImageUrl = $"https://picsum.photos/1200/900?image={i + 1}"
        });
      }
    }

    public Task<IEnumerable<ItemModel>> GetItems(int page = 1, int pageSize = 9)
    {
      return Task.FromResult(_results.Skip((page - 1) * pageSize).Take(pageSize));
    }

    public Task<ItemModel> GetItem(Guid id)
    {
      return Task.FromResult(_results.FirstOrDefault(x => x.Id == id));
    }

    public Task<ItemModel> CreateItem(ItemModel item)
    {
      return Task.FromResult(item);
    }
  }
}
