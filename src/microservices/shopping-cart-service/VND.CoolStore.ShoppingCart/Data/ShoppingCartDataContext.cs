using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Infrastructure.Data.EfCore.SqlServer;
using VND.CoolStore.ShoppingCart.Data.TypeConfig;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart.Data
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

    public class ShoppingCartDataContextDesignFactory : IDesignTimeDbContextFactory<ShoppingCartDataContext>
    {
        public ShoppingCartDataContext CreateDbContext(string[] args)
        {
            var dbConnFactory = new SqlServerDbConnStringFactory();
            dbConnFactory.SetBasePath(AppContext.BaseDirectory);
            var conn = dbConnFactory.Create();
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingCartDataContext>()
                .UseSqlServer(
                    conn,
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(GetType().Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    }
                );

            return new ShoppingCartDataContext(optionsBuilder.Options);
        }
    }
}
