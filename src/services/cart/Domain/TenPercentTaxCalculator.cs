using VND.CoolStore.Services.Cart.Infrastructure.Service;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class TenPercentTaxCalculator : TaxCalculatorContext
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
