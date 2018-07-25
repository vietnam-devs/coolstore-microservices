using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using VND.CoolStore.Shared.Catalog.GetProductById;
using VND.CoolStore.Shared.Catalog.GetProducts;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.Cart.Infrastructure.Service.Impl
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

    public async Task<GetProductByIdResponse> GetProductByIdAsync(GetProductByIdRequest request)
    {
      var getProductEndPoint = $"{_catalogServiceUri}/api/v1/products/{request.Id}";
      var response = await RestClient.GetAsync<GetProductByIdResponse>(getProductEndPoint);
      return response;
    }

    public async Task<IEnumerable<GetProductsResponse>> GetProductsAsync(GetProductsRequest request)
    {
      string getProductsEndPoint = $"{_catalogServiceUri}/api/v1/products";
      List<GetProductsResponse> responses = await RestClient.GetAsync<List<GetProductsResponse>>(getProductsEndPoint);
      return responses;
    }
  }
}
