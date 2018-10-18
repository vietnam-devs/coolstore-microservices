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
    private readonly HttpClient _httpClient;
    private readonly string _catalogUrl;

    public ItemService(ConfigModel config, HttpClient httpClient)
    {
      _httpClient = httpClient;
      _catalogUrl = $"{config.CatalogService}";
    }

    public async Task<Pagination<ItemModel>> GetItems(int page = 1, int pageSize = 9, long price = 4000)
    {
      var items = await _httpClient.GetJsonAsync<List<ItemModel>>($"{_catalogUrl}/api/products");

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
      return await _httpClient.GetJsonAsync<ItemModel>($"{_catalogUrl}/api/products/{id}");
    }

    public async Task<ItemModel> CreateItem(ItemModel item)
    {
      item.Id = Guid.NewGuid();
      await _httpClient.PostJsonAsync($"{_catalogUrl}/api/products", item);
      return await _httpClient.GetJsonAsync<ItemModel>($"{_catalogUrl}/api/products/{item.Id}");
    }
  }
}
