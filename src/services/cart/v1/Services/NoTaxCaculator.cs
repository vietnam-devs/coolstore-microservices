using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Services;

namespace VND.CoolStore.Services.Cart.v1.Services
{
  public class NoTaxCaculator : PriceCalculatorContext
  {
    public NoTaxCaculator(IPromoGateway promoGateway, IShippingGateway shippingGateway)
      : base(promoGateway, shippingGateway)
    {
    }

    protected override double AddTaxCost(double total)
    {
      return total + 0.0D;
    }
  }
}
