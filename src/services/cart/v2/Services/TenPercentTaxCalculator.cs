using NetCoreKit.Utils.Attributes;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Services;

namespace VND.CoolStore.Services.Cart.v2.Services
{
  [AutoScanAwareness]
  public class TenPercentTaxCalculator : PriceCalculatorContext
  {
    public TenPercentTaxCalculator(IPromoGateway promoGateway, IShippingGateway shippingGateway)
      : base(promoGateway, shippingGateway)
    {
    }

    protected override double AddTaxCost(double total)
    {
      return (total * 10 / 100) + total;
    }
  }
}
