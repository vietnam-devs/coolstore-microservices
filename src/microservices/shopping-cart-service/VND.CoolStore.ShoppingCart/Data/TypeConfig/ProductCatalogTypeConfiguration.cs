using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VND.CoolStore.ShoppingCart.Data.TypeConfig
{
    public class ProductCatalogConfiguration : IEntityTypeConfiguration<Domain.ProductCatalog.ProductCatalog>
    {
        public void Configure(EntityTypeBuilder<Domain.ProductCatalog.ProductCatalog> builder)
        {
            builder.ToTable("ProductCatalogs", "catalog");

            builder.HasKey(x => x.Id);
        }
    }
}
