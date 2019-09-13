/*using System;
using System.ComponentModel.DataAnnotations.Schema;
using CloudNativeKit.Domain;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ShoppingCart.Domain.ProductCatalog
{

    public sealed class ProductCatalog : AggregateRootBase<Guid>
    {
        private ProductCatalog(Guid productId)
            : this(GenerateId(), productId, string.Empty, 0.0D, string.Empty)
        {
        }

        private ProductCatalog(Guid productId, string name, double price, string desc)
            : this(GenerateId(), productId, name, price, desc)
        {
        }

        private ProductCatalog(Guid id, Guid productId, string name, double price, string desc)
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

        public static ProductCatalog Load(Guid productId)
        {
            return new ProductCatalog(productId);
        }

        public static ProductCatalog Load(Guid productId, string name, double price, string desc)
        {
            return new ProductCatalog(productId, name, price, desc);
        }

        public ProductCatalog LinkCartItem(CartItem cartItem)
        {
            CartItem = cartItem;
            return this;
        }
    }
}*/
