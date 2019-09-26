using System;
using CloudNativeKit.Domain;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ShoppingCart.Domain.Cart
{
    public sealed class CartItem : EntityBase<Guid>
    {
        private CartItem() : base(NewId())
        {
        }

        private CartItem(Guid id, Guid productId, int quantity, double promoSavings = 0.0D) : base(id)
        {
            Product = ProductCatalogId.Load(EmptyId(), productId);
            Quantity = quantity;
            PromoSavings = promoSavings;
        }

        public static CartItem Load(Guid id, Guid productId, int quantity, double promoSavings)
        {
            return new CartItem(id, productId, quantity, promoSavings);
        }

        public int Quantity { get; private set; }

        public double PromoSavings { get; private set; }

        public Cart Cart { get; private set; }

        public Guid CurrentCartId { get; private set; }

        public ProductCatalogId Product { get; private set; }

        public CartItem ChangePromoSavings(double promoSavings)
        {
            PromoSavings = promoSavings;
            return this;
        }

        public CartItem AccumulateQuantity(int quantity)
        {
            Quantity += quantity;
            return this;
        }
    }
}
