using System;
using System.Linq;
using VND.CoolStore.Services.Cart.Domain;

namespace VND.CoolStore.Services.Cart.Infrastructure.Services
{
  public abstract class PriceCalculatorContext : IPriceCalculator
  {
    private readonly IPromoGateway _promoGateway;
    private readonly IShippingGateway _shippingGateway;

    protected PriceCalculatorContext(IPromoGateway promoGateway, IShippingGateway shippingGateway)
    {
      _promoGateway = promoGateway;
      _shippingGateway = shippingGateway;
    }

    public Domain.Cart Execute(Domain.Cart cart)
    {
      if (cart == null)
      {
        throw new Exception("Cart is null.");
      }

      if (cart.CartItems != null && cart.CartItems?.Count() > 0)
      {
        cart.CartItemTotal = 0;
        foreach (var item in cart.CartItems)
        {
          cart.CartItemPromoSavings = cart.CartItemPromoSavings + (item.PromoSavings * item.Quantity);
          cart.CartItemTotal = cart.CartItemTotal + (item.Product.Price * item.Quantity);
        }
        cart = _shippingGateway.CalculateShipping(cart);
      }

      cart = _promoGateway.ApplyShippingPromotions(cart);
      cart.CartTotal = AddTaxCost(cart.CartItemTotal + cart.ShippingTotal);

      return cart;
    }

    protected abstract double AddTaxCost(double total);
  }
}
