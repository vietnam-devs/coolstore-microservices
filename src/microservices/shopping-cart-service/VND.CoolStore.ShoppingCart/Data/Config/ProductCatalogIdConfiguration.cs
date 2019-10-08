using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.Data.Config
{
    public class ProductCatalogIdConfiguration : IEntityTypeConfiguration<ProductCatalogId>
    {
        public void Configure(EntityTypeBuilder<ProductCatalogId> builder)
        {
            builder.ToTable("ProductCatalogIds", "cart");

            builder.HasKey(x => x.Id);

            builder.HasOne(pc => pc.CartItem)
                .WithOne(ci => ci.Product)
                .HasForeignKey<ProductCatalogId>(ci => ci.CurrentCartItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
