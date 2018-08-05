using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using VND.CoolStore.Services.Cart.Shared.Services;
using VND.CoolStore.Services.Cart.Shared.Services.Dtos;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.Cart.UseCases.v1.Services.Impl
{
  public class CatalogService : ProxyServiceBase, ICatalogService
  {
    private readonly string _catalogServiceUri;

    public CatalogService(
      RestClient rest,
      IConfiguration config,
      IHostingEnvironment env) : base(rest)
    {
      _catalogServiceUri = config.GetHostUri(env, "Catalog");
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
      var getProductEndPoint = $"{_catalogServiceUri}/api/v1/products/{id}";
      var response = await RestClient.GetAsync<ProductDto>(getProductEndPoint);
      return response;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
      var getProductsEndPoint = $"{_catalogServiceUri}/api/v1/products";
      var responses = await RestClient.GetAsync<List<ProductDto>>(getProductsEndPoint);
      return responses;
    }
  }
}
