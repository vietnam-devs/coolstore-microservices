using VND.CoolStore.Services.Cart.Infrastructure.Service;

namespace VND.CoolStore.Services.Cart.Domain
{
  public class NoTaxCaculator : TaxCalculatorContext
  {
    public NoTaxCaculator(IPromoService promoService, IShippingService shippingService)
      : base(promoService, shippingService)
    {
    }

    protected override double AddTaxCost(double total)
    {
      return total + 0.0D;
    }
  }
}
