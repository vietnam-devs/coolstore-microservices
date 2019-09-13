using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.DataPersistence.TypeConfig
{
    public class CartTypeConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts", "cart");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);

            builder.Property<bool>("IsCheckout").HasColumnName("IsCheckout");

            builder
                .HasMany(ci => ci.CartItems)
                .WithOne(c => c.Cart)
                .HasForeignKey(k => k.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
