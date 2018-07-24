using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Shared.Cart.GetCartById;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;
using VND.CoolStore.Shared.Catalog.GetProductById;
using VND.FW.Infrastructure.EfCore.Repository;
using VND.FW.Infrastructure.EfCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace VND.CoolStore.Services.Cart.Infrastructure.Service.Impl
{
  public class CartService : ICartService
  {
    private readonly ICatalogService _catalogService;
    private readonly IEfQueryRepository<Domain.Cart> _queryRepository;
    private readonly IEfRepositoryAsync<Domain.Cart> _mutateRepository;

    public CartService(
      IEfQueryRepository<Domain.Cart> queryRepository,
      IEfRepositoryAsync<Domain.Cart> mutateRepository,
      ICatalogService catalogService)
    {
      _catalogService = catalogService;
      _queryRepository = queryRepository;
      _mutateRepository = mutateRepository;
    }

    public TaxCalculatorContext TaxCalculator { get; set; }

    public async Task<GetCartByIdResponse> GetCartById(Guid id)
    {
      Domain.Cart cart = await _queryRepository.GetByIdAsync(
        id,
        cartQueryable => cartQueryable
          .Include(x => x.CartItems)
          .ThenInclude((CartItem cartItem) => cartItem.Product));

      cart = await InitCart(cart, populatePrice: true);
      cart = TaxCalculator.Execute(cart);

      return new GetCartByIdResponse
      {
        Id = cart.Id,
        CartTotal = cart.CartTotal,
        CartItemTotal = cart.CartItemTotal,
        CartItemPromoSavings = cart.CartItemPromoSavings,
        ShippingPromoSavings = cart.ShippingPromoSavings,
        ShippingTotal = cart.ShippingTotal,
        Items = cart.CartItems.Select(cc =>
        {
          return new GetCartByIdResponse.CartItemResponse
          {
            ProductId = cc.Product.Id,
            ProductName = cc.Product.Name,
            Price = cc.Price,
            Quantity = cc.Quantity,
            PromoSavings = cc.PromoSavings
          };
        }).ToList()
      };
    }

    public async Task<Domain.Cart> InsertItemToCart(InsertItemToNewCartRequest request)
    {
      Domain.Cart cart = await InitCart();

      cart.InsertItemToCart(new CartItem
      {
        Product = new Product(request.ProductId),
        PromoSavings = 0.0D,
        Quantity = request.Quantity
      });

      cart = TaxCalculator.Execute(cart);

      return await _mutateRepository.AddAsync(cart);
    }

    public async Task<bool> RemoveItemInCart(Guid cartId, Guid itemId)
    {
      Domain.Cart cart = await _queryRepository.GetByIdAsync(cartId);
      if (cart == null)
      {
        throw new Exception($"Could not find the cart[{cartId}]");
      }

      cart = await InitCart(cart);

      cart = cart.RemoveCartItem(itemId);
      bool isSucceed = await _mutateRepository.UpdateAsync(cart) != null;

      cart = TaxCalculator.Execute(cart);

      return isSucceed;
    }

    public async Task<Domain.Cart> UpdateItemInCart(UpdateItemInCartRequest request)
    {
      Domain.Cart cart = await _queryRepository.GetByIdAsync(request.CartId);
      if (cart == null)
      {
        throw new Exception($"Could not find the cart[{request.CartId}]");
      }

      cart = await InitCart(cart);

      CartItem item = cart.CartItems.FirstOrDefault(x => x.Product.Id == request.ItemId);
      if (item == null)
      {
        throw new Exception($"Could not find the item[{request.ItemId}]");
      }

      item.Quantity = request.Quantity;

      cart = TaxCalculator.Execute(cart);

      return await _mutateRepository.UpdateAsync(cart);
    }

    private async Task<Domain.Cart> InitCart(Domain.Cart cart = null, bool populatePrice = false)
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
          IEnumerable<GetProductByIdResponse> products =
            await _catalogService.GetProductByIdAsync(new GetProductByIdRequest { Id = item.Product.Id });

          if (products == null)
          {
            throw new Exception("Could not find product.");
          }

          GetProductByIdResponse product = products.FirstOrDefault();

          item.Product = new Product(product.Id, product.Name, product.Price, product.Desc);
          item.Price = product.Price;
          item.PromoSavings = 0;
        }
      }

      return cart;
    }
  }
}
