using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VND.CoolStore.ShoppingCart.Domain.ProductCatalog;

namespace VND.CoolStore.ShoppingCart.Data.TypeConfig
{
    public class ProductCatalogConfiguration : IEntityTypeConfiguration<ProductCatalog>
    {
        public void Configure(EntityTypeBuilder<ProductCatalog> builder)
        {
            builder.ToTable("ProductCatalogs", "catalog");

            builder.HasKey(x => x.Id);
        }
    }
}
