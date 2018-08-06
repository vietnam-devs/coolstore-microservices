using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.Infrastructure.Services;

namespace VND.CoolStore.Services.Cart.UseCases.v1.Services
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
