using NetCoreKit.Utils.Attributes;
using VND.CoolStore.Services.Cart.Domain;

namespace VND.CoolStore.Services.Cart.Infrastructure.Gateways
{
  [AutoScanAwareness]
  public class ShippingGateway : IShippingGateway
  {
    public Domain.Cart CalculateShipping(Domain.Cart cart)
    {
      // TODO: will calculate it later
      return cart;
    }
  }
}
