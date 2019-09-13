using System;
using CloudNativeKit.Domain;

namespace VND.CoolStore.ShoppingCart.Domain.Cart
{
    public sealed class ProductCatalogId : IdentityBase<Guid>
    {
        public ProductCatalogId(Guid id) : base(id)
        {
        }
    }
}
