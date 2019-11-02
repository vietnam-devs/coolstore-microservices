using System;
using System.Collections.Generic;
using System.IO;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VND.CoolStore.ProductCatalog.DataContracts.Dto.V1;
using VND.CoolStore.ShoppingCart.Domain.Cart;

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingCartDataContext).Assembly);

            //seed data
            var seedData = Path.GetFullPath("products.json", AppContext.BaseDirectory);
            using StreamReader sr = new StreamReader(seedData);
            var readData = sr.ReadToEnd();
            var productModels = JsonConvert.DeserializeObject<List<CatalogProductDto>>(readData);

            foreach(var prod in productModels)
            {
                modelBuilder.Entity<Domain.ProductCatalog.ProductCatalog>().HasData(
                    Domain.ProductCatalog.ProductCatalog.Load(
                        prod.Id.ConvertTo<Guid>(),
                        prod.InventoryId.ConvertTo<Guid>(),
                        prod.Name,
                        prod.Price,
                        prod.Desc,
                        prod.ImageUrl
                        )
                    )
                ;
            }
        }
    }
}
