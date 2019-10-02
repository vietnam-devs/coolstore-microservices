using System;
using CloudNativeKit.Domain;
using VND.CoolStore.ShoppingCart.Usecases.ReplicateProductCatalogInfo;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ShoppingCart.Domain.ProductCatalog
{
    public sealed class ProductCatalog : AggregateRootBase<Guid>
    {
        private ProductCatalog(Guid productId, Guid inventoryId)
            : this(NewId(), productId, inventoryId, string.Empty, 0.0D, string.Empty, "https://picsum.photos/1200/900?image=1")
        {
        }

        private ProductCatalog(Guid productId, Guid inventoryId, string name, double price, string desc, string imagePath)
            : this(NewId(), productId, inventoryId, name, price, desc, imagePath)
        {
        }

        private ProductCatalog(Guid id, Guid productId, Guid inventoryId, string name, double price, string desc, string imagePath)
        {
            Id = id;
            ProductId = productId;
            InventoryId = inventoryId;
            Name = name;
            Price = price;
            Desc = desc;
            ImagePath = imagePath;
            IsDeleted = false;
        }

        public Guid ProductId { get; private set; }

        public string Name { get; private set; }

        public double Price { get; private set; }

        public string Desc { get; private set; }

        public string ImagePath { get; private set; }

        public bool IsDeleted { get; private set; }

        public Guid InventoryId { get; private set; }

        public static ProductCatalog Load(Guid productId, Guid inventoryId)
        {
            return new ProductCatalog(productId, inventoryId);
        }

        public static ProductCatalog Load(Guid productId, Guid inventoryId, string name, double price, string desc, string imagePath = "https://picsum.photos/1200/900?image=1")
        {
            return new ProductCatalog(productId, inventoryId, name, price, desc, imagePath);
        }

        public ProductCatalog ReplicateProductCatalog(ReplicateProductCatalogInfo info)
        {
            Name = info.Name;
            Price = info.Price;
            Desc = info.Description;
            ImagePath = info.ImagePath;
            return this;
        }

        public ProductCatalog MarkAsDeleted()
        {
            IsDeleted = true;
            return this;
        }
    }
}
