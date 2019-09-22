using System;
using System.ComponentModel.DataAnnotations;
using CloudNativeKit.Domain;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ShoppingCart.Domain.Cart
{
    public sealed class CartItem : EntityBase<Guid>
    {
        private CartItem() : base(GenerateId())
        {
        }

        private CartItem(Guid id, int quantity, double price = 0.0D, double promoSavings = 0.0D) : base(id)
        {
            Quantity = quantity;
            Price = price;
            PromoSavings = promoSavings;
        }

        public int Quantity { get; private set; }

        public double Price { get; private set; }

        public double PromoSavings { get; private set; }

        public Cart Cart { get; private set; }
        public Guid CartId { get; private set; }

        public ProductCatalogId Product { get; private set; }

        public static CartItem Load(Guid productId, int quantity, double price = 0.0D, double promoSavings = 0.0D)
        {
            return Load(GenerateId(), productId, quantity, price, promoSavings);
        }

        public static CartItem Load(Guid id, Guid productId, int quantity, double price, double promoSavings)
        {
            return new CartItem(id, quantity, price, promoSavings);//.LinkProduct(productId);
        }

        public CartItem LinkCart(Cart cart)
        {
            Cart = cart;
            CartId = cart.Id;
            return this;
        }

        /*public CartItem LinkProduct(Guid productId)
        {
            var product = ProductCatalog.Load(productId);
            product.LinkCartItem(this);
            Product = product;
            return this;
        }

        public CartItem FillUpProductInfo(string name, double price, string desc)
        {
            Product = ProductCatalog.Load(Product.ProductId, name, price, desc);
            return this;
        }*/

        public CartItem ChangePrice(double price)
        {
            Price = price;
            return this;
        }

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
