using VND.CoolStore.Services.Cart.Shared.Services;

namespace VND.CoolStore.Services.Cart.UseCases.v1.Services.Impl
{
  public class NoTaxCaculator : PriceCalculatorContext
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
