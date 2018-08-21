using NetCoreKit.Utils.Attributes;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Services;

namespace VND.CoolStore.Services.Cart.v1.Services
{
  public interface INoTaxPriceCalculator : IPriceCalculator
  {
  }

  [AutoScanAwareness]
  public class NoTaxCaculator : PriceCalculatorContext, INoTaxPriceCalculator
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
