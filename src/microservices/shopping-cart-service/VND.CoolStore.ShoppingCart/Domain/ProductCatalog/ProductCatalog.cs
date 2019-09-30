using System;
using CloudNativeKit.Domain;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ShoppingCart.Domain.ProductCatalog
{
    public sealed class ProductCatalog : AggregateRootBase<Guid>
    {
        private ProductCatalog(Guid productId)
            : this(NewId(), productId, string.Empty, 0.0D, string.Empty, "https://picsum.photos/1200/900?image=1")
        {
        }

        private ProductCatalog(Guid productId, string name, double price, string desc, string imagePath)
            : this(NewId(), productId, name, price, desc, imagePath)
        {
        }

        private ProductCatalog(Guid id, Guid productId, string name, double price, string desc, string imagePath)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Price = price;
            Desc = desc;
            ImagePath = imagePath;
        }

        public Guid ProductId { get; private set; }

        public string Name { get; private set; }

        public double Price { get; private set; }

        public string Desc { get; private set; }

        public string ImagePath { get; private set; }

        public static ProductCatalog Load(Guid productId)
        {
            return new ProductCatalog(productId);
        }

        public static ProductCatalog Load(Guid productId, string name, double price, string desc, string imagePath = "https://picsum.photos/1200/900?image=1")
        {
            return new ProductCatalog(productId, name, price, desc, imagePath);
        }

        public ProductCatalog SyncData(ProductUpdated @event)
        {
            Name = @event.Name;
            Price = @event.Price;
            Desc = @event.Desc;
            ImagePath = @event.ImageUrl;
            return this;
        }
    }
}
