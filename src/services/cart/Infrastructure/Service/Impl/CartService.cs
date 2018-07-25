using System;
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
using VND.CoolStore.Shared.Cart.Checkout;

namespace VND.CoolStore.Services.Cart.Infrastructure.Service.Impl
{
  public class CartService : ICartService
  {
    private readonly ICatalogService _catalogService;
    private readonly IEfQueryRepository<Domain.Cart> _queryRepository;
    private readonly IEfRepositoryAsync<Domain.Cart> _mutateRepository;
    private readonly IEfRepositoryAsync<CartItem> _cartItemMutateRepository;

    public CartService(
      IEfQueryRepository<Domain.Cart> queryRepository,
      IEfRepositoryAsync<Domain.Cart> mutateRepository,
      IEfRepositoryAsync<CartItem> cartItemMutateRepository,
      ICatalogService catalogService)
    {
      _catalogService = catalogService;
      _queryRepository = queryRepository;
      _mutateRepository = mutateRepository;
      _cartItemMutateRepository = cartItemMutateRepository;
    }

    public PriceCalculatorContext PriceCalculatorContext { get; set; }

    public async Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request)
    {
      var cart = await GetCart(request.Id);

      cart.IsCheckout = true;
      var checkoutCart = await _mutateRepository.UpdateAsync(cart);
      return new CheckoutResponse
      {
        IsSucceed = checkoutCart != null
      };
    }

    public async Task<GetCartByIdResponse> GetCartByIdAsync(Guid id)
    {
      var cart = await GetCart(id);
      cart = await InitCart(cart, populatePrice: true);
      cart = PriceCalculatorContext.Execute(cart);

      return GetCartByIdResponse(cart);
    }

    public async Task<GetCartByIdResponse> InsertItemToCartAsync(InsertItemToNewCartRequest request)
    {
      var cart = new Domain.Cart();
      cart.InsertItemToCart(new CartItem
      {
        Product = new Product(request.ProductId),
        PromoSavings = 0.0D,
        Quantity = request.Quantity
      });
      
      cart = await InitCart(cart, populatePrice: true);
      cart = PriceCalculatorContext.Execute(cart);

      await _mutateRepository.AddAsync(cart);
      return GetCartByIdResponse(cart);
    }

    public async Task<bool> RemoveItemInCartAsync(Guid cartId, Guid productId)
    {
      var cart = await GetCart(cartId);
      cart = await InitCart(cart);

      var cartItem = cart.CartItems.FirstOrDefault(x => x.Product.ProductId == productId);
      if(cartItem == null)
      {
        throw new Exception($"Could not find CartItem {cartItem.Id}");
      }

      cart = cart.RemoveCartItem(cartItem.Id);
      var isSucceed = await _mutateRepository.UpdateAsync(cart) != null;
      await _cartItemMutateRepository.DeleteAsync(cartItem);

      cart = PriceCalculatorContext.Execute(cart);

      return isSucceed;
    }

    public async Task<GetCartByIdResponse> UpdateItemInCartAsync(UpdateItemInCartRequest request)
    {
      var isNewItem = false;
      var cart = await GetCart(request.CartId);
      cart = await InitCart(cart);

      var item = cart.CartItems.FirstOrDefault(x => x.Product.ProductId == request.ProductId);

      // if not exists then it should be a new item
      if (item == null)
      {
        isNewItem = true;
        item = new CartItem()
        {
          Quantity = request.Quantity,
          Product = new Product(request.ProductId)
        };
        cart.CartItems.Add(item);
      }
      else
      {
        // otherwise is updating the current item in the cart
        item.Quantity = request.Quantity;
      }

      cart = PriceCalculatorContext.Execute(cart);
      var result = await _mutateRepository.UpdateAsync(cart);

      // Todo: refactor to unit of work later
      if (!isNewItem)
      {
        await _cartItemMutateRepository.UpdateAsync(item);
      } else
      {
        await _cartItemMutateRepository.AddAsync(item);
      }

      return GetCartByIdResponse(cart);
    }

    private async Task<Domain.Cart> GetCart(Guid cartId)
    {
      var cart = await _queryRepository.GetByIdAsync(
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

    private GetCartByIdResponse GetCartByIdResponse(Domain.Cart cart)
    {
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
            ProductId = cc.Product.ProductId,
            ProductName = cc.Product.Name,
            Price = cc.Price,
            Quantity = cc.Quantity,
            PromoSavings = cc.PromoSavings
          };
        }).ToList()
      };
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
          var product =
            await _catalogService.GetProductByIdAsync(new GetProductByIdRequest { Id = item.Product.ProductId });

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
