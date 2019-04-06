using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Cart.Infrastructure.Db
{
    public class CartDbContext : AppDbContext
    {
        public CartDbContext(DbContextOptions options, IConfiguration config, IDomainEventDispatcher eventBus = null)
            : base(options, config, eventBus)
        {
        }
    }

    public class CartDbContextDesignFactory : IDesignTimeDbContextFactory<CartDbContext>
    {
        public CartDbContext CreateDbContext(string[] args)
        {
            var config = ConfigurationHelper.GetConfiguration();
            var dbSection = config.GetSection("Features:EfCore:MySqlDb");
            var connPattern = dbSection["ConnString"];
            var connConfigs = dbSection["FQDN"].Split(':');
            var username = dbSection["UserName"];
            var password = dbSection["Password"];
            var database = dbSection["Database"];
            var databaseInfo = dbSection["DbInfo"];

            var fqdn = connConfigs?.First();
            var port = connConfigs?.Except(new[] { fqdn }).First();

            var conn = string.Format(
                connPattern,
                fqdn, port,
                username,
                password,
                database);

            var optionsBuilder = new DbContextOptionsBuilder<CartDbContext>()
                .UseMySql(
                    conn, sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(GetType().Assembly.FullName);
                        sqlOptions.ServerVersion(databaseInfo);
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    }
                );

            return new CartDbContext(optionsBuilder.Options, config);
        }
    }

    public class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfiguration(string basePath = null)
        {
            basePath = basePath ?? Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
