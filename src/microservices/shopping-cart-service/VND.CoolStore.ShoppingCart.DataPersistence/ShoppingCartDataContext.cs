using System.Collections.Generic;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db;
using Microsoft.EntityFrameworkCore;
using VND.CoolStore.ShoppingCart.DataPersistence.TypeConfig;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.DataPersistence
{
    public class ShoppingCartDataContext : AppDbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        //public DbSet<ProductCatalog> ProductCatalogs { get; set; }

        public ShoppingCartDataContext(DbContextOptions options, IEnumerable<IDomainEventDispatcher> eventBuses = null)
            : base(options, eventBuses)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
