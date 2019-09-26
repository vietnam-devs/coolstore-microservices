using System;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using Microsoft.EntityFrameworkCore;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.Data.Repositories
{
    public static class CartRepositoryExtensions
    {
        public static async Task<Cart> GetFullCartAsync(this IQueryRepository<Cart, Guid> cartRepo,
            Guid cartId,
            bool disableTracking = true)
        {
            var cart = await cartRepo.GetByIdAsync<ShoppingCartDataContext, Cart, Guid>(
                cartId,
                cartQueryable => cartQueryable
                    .Include(x => x.CartItems)
                    .ThenInclude(cartItem => cartItem.Product),
                !disableTracking);

            if (cart == null)
                throw new Exception($"Could not find the cart[{cartId}].");

            return cart;
        }
    }
}
