using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using WebUI.Model;

namespace WebUI.Services
{
  public class ItemService : BaseService
  {
    private readonly string _catalogUrl;

    public ItemService(AppState appState, ConfigModel config, HttpClient httpClient)
      : base(appState, config, httpClient)
    {
      _catalogUrl = $"{Config.CatalogService}";
    }

    public async Task<Pagination<ItemModel>> GetItems(int page = 1, int pageSize = 9, long price = 4000)
    {
      await SetHeaderToken();

      var items = await RestClient.GetJsonAsync<List<ItemModel>>($"{_catalogUrl}/api/products");

      var itemsFilter = items.Where(x => x.Price <= price).ToList();
      var pagination = new Pagination<ItemModel>
      {
        Items = itemsFilter.Skip((page - 1) * pageSize).Take(pageSize),
        CurrentPage = page,
        TotalItems = itemsFilter.Count(),
        PageSize = pageSize
      };

      return pagination;
    }

    public async Task<ItemModel> GetItem(Guid id)
    {
      await SetHeaderToken();
      return await RestClient.GetJsonAsync<ItemModel>($"{_catalogUrl}/api/products/{id}");
    }

    public async Task CreateItem(ItemModel item)
    {
      await SetHeaderToken();
      item.Id = Guid.NewGuid();
      await RestClient.PostJsonAsync($"{_catalogUrl}/api/products", item);
    }
  }
}
