using System;
using System.Threading.Tasks;

namespace VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart
{
  public interface IDeleteItemService
  {
    Task<bool> Execute(Guid cartId, Guid productId);
  }
}
