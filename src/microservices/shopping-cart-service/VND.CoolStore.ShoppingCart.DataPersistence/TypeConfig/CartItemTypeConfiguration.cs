using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.DataPersistence.TypeConfig
{
    public class CartItemTypeConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems", "cart");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);

            //builder.Property(x => x.Product.Id);
            builder.Property(x => x.Quantity);
            builder.Property(x => x.Price);
            //builder.Property(x => x.CartId);
        }
    }
}
