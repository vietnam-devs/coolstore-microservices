using System;
using CloudNativeKit.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VND.CoolStore.ShoppingCart.Data
{
    public class ShoppingCartDataContextDesignFactory : IDesignTimeDbContextFactory<ShoppingCartDataContext>
    {
        public ShoppingCartDataContext CreateDbContext(string[] args)
        {
            var connString = ConfigurationHelper.GetConfiguration(AppContext.BaseDirectory)?.GetConnectionString("MainDb");
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingCartDataContext>()
                .UseSqlServer(
                    connString,
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
