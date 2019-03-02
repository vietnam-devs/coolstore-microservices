using VND.CoolStore.Services.Cart.Domain;

namespace VND.CoolStore.Services.Cart.Infrastructure.Gateways
{
    public class PromoGateway : IPromoGateway
    {
        public Domain.Cart ApplyCartItemPromotions(Domain.Cart cart)
        {
            // TODO: will calculate it later
            return cart;
        }

        public Domain.Cart ApplyShippingPromotions(Domain.Cart cart)
        {
            // TODO: will calculate it later
            return cart;
        }
    }
}
