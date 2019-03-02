using System;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreKit.Domain;
using static NetCoreKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.Services.Cart.Domain
{
    public sealed class Product : IdentityBase
    {
        private Product()
        {
        }

        private Product(Guid productId)
            : this(productId, string.Empty, 0.0D, string.Empty)
        {
        }

        private Product(Guid productId, string name, double price, string desc)
            : this(GenerateId(), productId, name, price, desc)
        {
        }

        private Product(Guid id, Guid productId, string name, double price, string desc)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Price = price;
            Desc = desc;
        }

        public Guid ProductId { get; }

        [NotMapped] public string Name { get; }

        [NotMapped] public double Price { get; }

        [NotMapped] public string Desc { get; }

        public CartItem CartItem { get; private set; }

        public static Product Load(Guid productId)
        {
            return new Product(productId);
        }

        public static Product Load(Guid productId, string name, double price, string desc)
        {
            return new Product(productId, name, price, desc);
        }

        public Product LinkCartItem(CartItem cartItem)
        {
            CartItem = cartItem;
            return this;
        }
    }
}
