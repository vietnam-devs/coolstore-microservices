using System.Collections.Generic;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using Microsoft.EntityFrameworkCore;
using VND.CoolStore.ShoppingCart.Data.TypeConfig;
using VND.CoolStore.ShoppingCart.Domain.Cart;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ShoppingCart.Data
{
    public class ShoppingCartDataContext : AppDbContext
    {
        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ProductCatalogId> ProductCatalogIds { get; set; }

        public DbSet<Domain.ProductCatalog.ProductCatalog> ProductCatalogs { get; set; }

        public ShoppingCartDataContext(DbContextOptions<ShoppingCartDataContext> options, IEnumerable<IDomainEventDispatcher> eventBuses = null)
            : base(options, eventBuses)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // cart schema
            modelBuilder.ApplyConfiguration(new CartTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCatalogIdTypeConfiguration());

            // catalog schema
            modelBuilder.ApplyConfiguration(new ProductCatalogConfiguration());

            base.OnModelCreating(modelBuilder);

            //seed data
            modelBuilder.Entity<Domain.ProductCatalog.ProductCatalog>().HasData(
                Domain.ProductCatalog.ProductCatalog.Load(NewId("05233341-185A-468A-B074-00EBD08559AA"), "tempor incididunt ut labore et do", 638, "quis nostrud exercitation ull"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("3CB275C5-AA53-40FF-BC6A-015327053AF9"), "m", 671, "sin")
                );
        }
    }
}
