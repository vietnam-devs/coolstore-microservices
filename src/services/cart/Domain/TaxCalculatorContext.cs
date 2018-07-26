using System;
using System.Linq;
using VND.CoolStore.Services.Cart.Infrastructure.Service;

namespace VND.CoolStore.Services.Cart.Domain
{
  public abstract class PriceCalculatorContext : IPriceCalculator
  {
    private readonly IPromoService _promoService;
    private readonly IShippingService _shippingService;

    protected PriceCalculatorContext(IPromoService promoService, IShippingService shippingService)
    {
      _promoService = promoService;
      _shippingService = shippingService;
    }

    public Cart Execute(Cart cart)
    {
      if (cart == null)
      {
        throw new Exception("Cart is null.");
      }

      if (cart.CartItems != null && cart.CartItems?.Count() > 0)
      {
        cart.CartItemTotal = 0;
        foreach (CartItem item in cart.CartItems)
        {
          cart.CartItemPromoSavings = cart.CartItemPromoSavings + (item.PromoSavings * item.Quantity);
          cart.CartItemTotal = cart.CartItemTotal + (item.Product.Price * item.Quantity);
        }
        cart = _shippingService.CalculateShipping(cart);
      }

      cart = _promoService.ApplyShippingPromotions(cart);
      cart.CartTotal = AddTaxCost(cart.CartItemTotal + cart.ShippingTotal);

      return cart;
    }

    protected abstract double AddTaxCost(double total);
  }
}
