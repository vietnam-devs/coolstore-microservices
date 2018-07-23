using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Shared.Catalog.GetProductById;
using VND.CoolStore.Shared.Catalog.GetProducts;

namespace VND.CoolStore.Services.ApiGateway.Infrastructure.Service
{
  public interface ICatalogService
  {
    Task<GetProductByIdResponse> GetProductByIdAsync(GetProductByIdRequest request);
    Task<IEnumerable<GetProductsResponse>> GetProductsAsync(GetProductsRequest request);
  }
}
