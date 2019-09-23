using System;
using CloudNativeKit.Infrastructure;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VND.CoolStore.ShoppingCart.Data
{
    public class MessagingDataContextDesignFactory : IDesignTimeDbContextFactory<MessagingDataContext>
    {
        public MessagingDataContext CreateDbContext(string[] args)
        {
            var connString = ConfigurationHelper.GetConfiguration(AppContext.BaseDirectory)?.GetConnectionString("MainDb");
            var optionsBuilder = new DbContextOptionsBuilder<MessagingDataContext>()
                .UseSqlServer(
                    connString,
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(GetType().Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    }
                );

            return new MessagingDataContext(optionsBuilder.Options);
        }
    }
}
