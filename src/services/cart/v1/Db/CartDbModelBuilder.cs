using Microsoft.EntityFrameworkCore;
using NetCoreKit.Infrastructure.EfCore.Db;
using VND.CoolStore.Services.Cart.Domain;

namespace VND.CoolStore.Services.Cart.v1.Db
{
    public class CartDbModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Cart>(b =>
            {
                b.HasMany(ci => ci.CartItems).WithOne(c => c.Cart)
                    .HasForeignKey(k => k.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(b =>
            {
                b.HasOne(x => x.Product).WithOne(y => y.CartItem)
                    .HasForeignKey<Product>(k => k.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
