using System;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Extensions;
using Microsoft.EntityFrameworkCore;
using VND.CoolStore.Services.Cart.Infrastructure.Dtos;

namespace VND.CoolStore.Services.Cart.v1.Services
{
  public abstract class CartServiceBase 
  {
    private readonly ICatalogGateway _catalogGateway;
    protected CartServiceBase(ICatalogGateway catalogGateway)
    {
      _catalogGateway = catalogGateway;
    }

    public virtual IEfQueryRepository<Domain.Cart> GetQueryRepository() => 
      throw new NotImplementedException("[CS] We need to override this method!");

    protected async Task<Domain.Cart> GetCart(Guid cartId)
    {
      var cart = await GetQueryRepository().GetByIdAsync(
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

    protected CartDto GetCartByIdResponse(Domain.Cart cart)
    {
      return new CartDto
      {
        Id = cart.Id,
        CartTotal = cart.CartTotal,
        CartItemTotal = cart.CartItemTotal,
        CartItemPromoSavings = cart.CartItemPromoSavings,
        ShippingPromoSavings = cart.ShippingPromoSavings,
        ShippingTotal = cart.ShippingTotal,
        IsCheckout = cart.IsCheckout,
        Items = cart.CartItems.Select(cc =>
        {
          return new CartDto.CartItemDto
          {
            ProductId = cc.Product.ProductId,
            ProductName = cc.Product.Name,
            Price = cc.Price,
            Quantity = cc.Quantity,
            PromoSavings = cc.PromoSavings
          };
        }).ToList()
      };
    }

    protected async Task<Domain.Cart> InitCart(Domain.Cart cart = null, bool populatePrice = false)
    {
      if (cart == null)
      {
        cart = new Domain.Cart();
      }

      if (populatePrice == false)
      {
        cart.CartItemPromoSavings = 0;
        cart.CartTotal = 0;
        cart.ShippingPromoSavings = 0;
        cart.ShippingTotal = 0;
        cart.CartItemTotal = 0;
      }

      if (cart.CartItems != null)
      {
        foreach (CartItem item in cart.CartItems)
        {
          var product = await _catalogGateway.GetProductByIdAsync(item.Product.ProductId);

          if (product == null)
          {
            throw new Exception("Could not find product.");
          }

          item.Product = new Product(product.Id, product.Name, product.Price, product.Desc);
          item.Price = product.Price;
          item.PromoSavings = 0;
        }
      }

      return cart;
    }
  }
}
