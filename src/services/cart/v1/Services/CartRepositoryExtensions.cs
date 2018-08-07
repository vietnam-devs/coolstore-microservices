using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VND.CoolStore.Services.Cart.Domain;
using VND.FW.Infrastructure.EfCore.Extensions;
using VND.FW.Infrastructure.EfCore.Repository;

namespace VND.CoolStore.Services.Cart.v1.Services
{
  public static class CartRepositoryExtensions
  {
    public static async Task<Domain.Cart> GetFullCart(this IEfQueryRepository<Domain.Cart> cartRepo, Guid cartId)
    {
      var cart = await cartRepo.GetByIdAsync(
        cartId,
        cartQueryable => cartQueryable
          .Include(x => x.CartItems)
          .ThenInclude((CartItem cartItem) => cartItem.Product));

      if (cart == null)
      {
        throw new Exception($"Could not find the cart[{cartId}]");
      }

      return cart;
    }
  }
}
