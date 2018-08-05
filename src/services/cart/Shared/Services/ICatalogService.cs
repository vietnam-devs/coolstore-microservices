using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Shared.Services.Dtos;

namespace VND.CoolStore.Services.Cart.Shared.Services
{
  public interface ICatalogService
  {
    Task<ProductDto> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetProductsAsync();
  }
}
