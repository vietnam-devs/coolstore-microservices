using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using WebUI.Model;

namespace WebUI.Services
{
  public class ItemService
  {
    private readonly ConfigModel _config;
    private readonly HttpClient _httpClient;

    public ItemService(ConfigModel config, HttpClient httpClient)
    {
      _config = config;
      _httpClient = httpClient;
    }

    public async Task<Pagination<ItemModel>> GetItems(int page = 1, int pageSize = 9, long price = 4000)
    {
      var uri = $"{_config.CatalogService}/api/products";
      var items = await _httpClient.GetJsonAsync<List<ItemModel>>(uri);

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
      var uri = $"{_config.CatalogService}/api/products/{id}";
      return await _httpClient.GetJsonAsync<ItemModel>(uri);
    }

    public async Task<ItemModel> CreateItem(ItemModel item)
    {
      var uri = $"{_config.CatalogService}/api/products";
      await _httpClient.PostJsonAsync(uri, item);
      return await _httpClient.GetJsonAsync<ItemModel>(uri);
    }
  }
}
