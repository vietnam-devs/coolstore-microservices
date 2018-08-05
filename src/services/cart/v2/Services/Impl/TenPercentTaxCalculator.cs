using VND.CoolStore.Services.Cart.Shared.Services;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class TenPercentTaxCalculator : PriceCalculatorContext
  {
    public TenPercentTaxCalculator(IPromoService promoService, IShippingService shippingService)
      : base(promoService, shippingService)
    {
    }

    protected override double AddTaxCost(double total)
    {
      return (total * 10 / 100) + total;
    }
  }
}
