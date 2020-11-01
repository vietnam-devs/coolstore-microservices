using System;
using N8T.Domain;

namespace ShoppingCartService.Domain.Model
{
    public class ShoppingCartItem : EntityBase
    {
        private ShoppingCartItem(Guid cartId, Guid id, Guid productId, int quantity, double promoSavings = 0.0D)
        {
            CartId = cartId;
            Id = id;
            ProductId = productId;
            Quantity = quantity;
            PromoSavings = promoSavings;
        }

        public static ShoppingCartItem Load(Guid cartId, Guid id, Guid productId, int quantity, double promoSavings)
        {
            return new ShoppingCartItem(cartId, id, productId, quantity, promoSavings);
        }

        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public double PromoSavings { get; set; }

        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public ShoppingCartItem ChangePromoSavings(double promoSavings)
        {
            PromoSavings = promoSavings;
            return this;
        }

        public ShoppingCartItem AccumulateQuantity(int quantity)
        {
            Quantity += quantity;
            return this;
        }
    }
}
