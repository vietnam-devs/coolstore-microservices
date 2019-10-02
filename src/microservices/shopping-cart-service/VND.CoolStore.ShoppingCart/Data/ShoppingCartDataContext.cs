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
                Domain.ProductCatalog.ProductCatalog.Load(NewId("05233341-185A-468A-B074-00EBD08559AA"), NewId("90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD"), "tempor incididunt ut labore et do", 638, "quis nostrud exercitation ull"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("3CB275C5-AA53-40FF-BC6A-015327053AF9"), NewId("90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD"), "m", 671, "sin"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("A162B9EE-85B4-457A-93FC-015DF74201DD"), NewId("90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD"), "ut labore et dolore magna aliqua. Ut enim ad minim ", 901, "dolor sit amet, consectetur adipiscing e"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("FF58A71D-76A2-41F8-AF44-018969694A59"), NewId("90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD"), "non proident, sunt in culpa qui officia deserunt mollit anim id", 661, "est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididun"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("9032B448-61F2-45F8-9E95-020961441613"), NewId("90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD"), "tempor incididunt ut labore ", 80, "ipsum dolor sit amet, consectetur adipis"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("D16E6353-0F88-43BA-9303-0241672D6AB6"), NewId("B8B62196-6369-409D-B709-11C112DD023F"), "aliqua. Ut enim ad minim veniam, quis nostrud exercitation ul", 275, "officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipi"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("80258882-2A90-4038-AC48-0283BB0AC9B7"), NewId("B8B62196-6369-409D-B709-11C112DD023F"), "velit esse cillum dolore eu fugiat ", 738, "mollit anim id est laborum.Lo"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("A11128B0-DD82-4179-99D9-0288E22DB70B"), NewId("B8B62196-6369-409D-B709-11C112DD023F"), "aute irure dolor in re", 51, "elit, sed do eiusmod tempor incididunt ut labore et dolore magna a"),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("E96A0646-6508-4E40-A035-0294E3B6A017"), NewId("B8B62196-6369-409D-B709-11C112DD023F"), "cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsu", 847, "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui "),
                Domain.ProductCatalog.ProductCatalog.Load(NewId("D39650D3-7929-4702-BCB9-02978D2C2711"), NewId("B8B62196-6369-409D-B709-11C112DD023F"), "consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna a", 2, "voluptate velit esse cillum dolore eu fugiat nulla pariat")
                );
        }
    }
}
