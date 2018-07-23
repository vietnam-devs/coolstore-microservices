using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VND.CoolStore.Services.Cart.Infrastructure.Service;
using VND.CoolStore.Shared.Cart.GetCartById;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;
using VND.CoolStore.Shared.Catalog.GetProductById;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.EfCore.Extensions;
using VND.FW.Infrastructure.EfCore.Repository;

namespace VND.CoolStore.Services.Cart.UseCases.v1
{
  [ApiVersion("1.0")]
  [Route("api/v{api-version:apiVersion}/carts")]
  public class CartController : EfCoreControllerBase<Domain.Cart>
  {
    private readonly ICatalogService _catalogService;
    private readonly IPromoService _promoService;
    private readonly IShippingService _shippingService;

    public CartController(
        IEfQueryRepository<Domain.Cart> queryRepository,
        IEfRepositoryAsync<Domain.Cart> mutateRepository,
        ICatalogService catalogService,
        IPromoService promoService,
        IShippingService shippingService)
        : base(queryRepository, mutateRepository)
    {
      _catalogService = catalogService;
      _promoService = promoService;
      _shippingService = shippingService;
    }

    [HttpGet(Name = nameof(GetCartById))]
    [Route("{id}")]
    public async Task<GetCartByIdResponse> GetCartById(Guid id)
    {
      Domain.Cart cart = await QueryRepository.GetByIdAsync(
        id,
        cartQueryable =>
         cartQueryable.Include(x => x.CartItems)
           .ThenInclude((Domain.CartItem cartItem) => cartItem.Product));

      cart = await InitCart(cart, populatePrice: true);
      cart = CalculateCart(cart);

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

    [HttpPost]
    [Route("insert-item")]
    public async Task<Domain.Cart> InsertItemToCart([FromBody] InsertItemToNewCartRequest request)
    {
      Domain.Cart cart = await InitCart();

      cart.InsertItemToCart(new Domain.CartItem
      {
        Product = new Domain.Product(request.ProductId),
        PromoSavings = 0.0D,
        Quantity = request.Quantity
      });

      cart = CalculateCart(cart);

      return await MutateRepository.AddAsync(cart);
    }

    [HttpPut]
    [Route("update-item")]
    public async Task<Domain.Cart> UpdateItemInCart([FromBody] UpdateItemInCartRequest request)
    {
      Domain.Cart cart = await QueryRepository.GetByIdAsync(request.CartId);
      if (cart == null)
      {
        throw new Exception($"Could not find the cart[{request.CartId}]");
      }

      cart = await InitCart(cart);

      Domain.CartItem item = cart.CartItems.FirstOrDefault(x => x.Product.Id == request.ItemId);
      if (item == null)
      {
        throw new Exception($"Could not find the item[{request.ItemId}]");
      }

      item.Quantity = request.Quantity;

      cart = CalculateCart(cart);

      return await MutateRepository.UpdateAsync(cart);
    }

    [HttpDelete]
    [Route("{cartId:guid}/item/{itemId:guid}")]
    public async Task<bool> RemoveItemInCart(Guid cartId, Guid itemId)
    {
      Domain.Cart cart = await QueryRepository.GetByIdAsync(cartId);
      if (cart == null)
      {
        throw new Exception($"Could not find the cart[{cartId}]");
      }

      cart = await InitCart(cart);

      cart = cart.RemoveCartItem(itemId);
      bool isSucceed = await MutateRepository.UpdateAsync(cart) != null;

      cart = CalculateCart(cart);

      return isSucceed;
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
        foreach (Domain.CartItem item in cart.CartItems)
        {
          IEnumerable<GetProductByIdResponse> products = await _catalogService.GetProductByIdAsync(new GetProductByIdRequest { Id = item.Product.Id });
          if (products == null)
          {
            throw new Exception("Could not find product.");
          }

          GetProductByIdResponse product = products.FirstOrDefault();

          item.Product = new Domain.Product(product.Id, product.Name, product.Price, product.Desc);
          item.Price = product.Price;
          item.PromoSavings = 0;
        }
      }

      return cart;
    }

    private Domain.Cart CalculateCart(Domain.Cart cart)
    {
      if (cart == null)
      {
        throw new Exception("Cart is null.");
      }

      if (cart.CartItems != null && cart.CartItems?.Count() > 0)
      {
        foreach (Domain.CartItem item in cart.CartItems)
        {
          cart.CartItemPromoSavings = cart.CartItemPromoSavings + (item.PromoSavings * item.Quantity);
          cart.CartItemTotal = cart.CartItemTotal + (item.Product.Price * item.Quantity);
        }
        cart = _shippingService.CalculateShipping(cart);
      }

      cart = _promoService.ApplyShippingPromotions(cart);
      cart.CartTotal = cart.CartItemTotal + cart.ShippingTotal;

      return cart;
    }
  }
}
