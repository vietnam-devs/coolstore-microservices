using System;
using CloudNativeKit.Domain;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ShoppingCart.Domain.Cart
{
    public sealed class ProductCatalogId : IdentityBase<Guid>
    {
        public ProductCatalogId(Guid productId) : this(NewId(), productId)
        {
        }

        public ProductCatalogId(Guid id, Guid productId) : base(id)
        {
            ProductId = productId;
        }

        public static ProductCatalogId Load(Guid id, Guid productId)
        {
            return new ProductCatalogId(id, productId);
        }

        public Guid ProductId { get; private set; }

        public CartItem CartItem { get; private set; }

        public Guid CurrentCartItemId { get; private set; }
    }
}
