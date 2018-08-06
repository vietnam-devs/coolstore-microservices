using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Infrastructure.Dtos;

namespace VND.CoolStore.Services.Cart.Infrastructure.Gateways
{
  public interface ICatalogGateway
  {
    Task<ProductDto> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetProductsAsync();
  }
}
