using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Infrastructure.Dtos;

namespace VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart
{
  public interface IUpdateItemService
  {
    Task<CartDto> Execute(UpdateItemInCartRequest request);
  }
}
