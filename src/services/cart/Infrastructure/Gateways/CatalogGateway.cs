using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Domain.Dtos;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.Cart.Infrastructure.Gateways
{
  public class CatalogGateway : ProxyServiceBase, ICatalogGateway
  {
    private readonly string _catalogServiceUri;

    public CatalogGateway(
      RestClient rest,
      IConfiguration config,
      IHostingEnvironment env) : base(rest)
    {
      _catalogServiceUri = config.GetHostUri(env, "Catalog");
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id)
    {
      var getProductEndPoint = $"{_catalogServiceUri}/api/products/{id}";
      var response = await RestClient.GetAsync<ProductDto>(getProductEndPoint);
      return response;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
      var getProductsEndPoint = $"{_catalogServiceUri}/api/products";
      var responses = await RestClient.GetAsync<List<ProductDto>>(getProductsEndPoint);
      return responses;
    }
  }
}
