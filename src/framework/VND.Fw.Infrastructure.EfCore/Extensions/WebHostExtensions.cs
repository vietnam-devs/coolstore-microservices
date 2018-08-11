using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VND.Fw.Infrastructure.EfCore.Migration;
using VND.Fw.Utils.Extensions;

namespace VND.Fw.Infrastructure.EfCore.Extensions
{
  public static class WebHostExtensions
  {
    public static IServiceProvider MigrateDbContext<TDbContext>(this IServiceProvider serviceProvider)
      where TDbContext : DbContext
    {
      return serviceProvider.MigrateDbContext<TDbContext>((context, services) =>
      {
        InstanceSeedData(services, context, typeof(ISeedData<>));
      });
    }

    public static IWebHost RegisterDbContext<TDbContext>(this IWebHost webHost)
      where TDbContext : DbContext
    {
      return webHost
        .MigrateDbContext<TDbContext>((context, services) =>
        {
          InstanceSeedData(services, context, typeof(ISeedData<>));
        });
    }

    internal static IWebHost MigrateDbContext<TContext>(this IWebHost webHost,
      Action<TContext, IServiceProvider> seeder)
      where TContext : DbContext
    {
      using (var scope = webHost.Services.CreateScope())
      {
        scope.ServiceProvider.MigrateDbContext(seeder);
      }

      return webHost;
    }

    public static void InstanceSeedData(this IServiceProvider resolver, DbContext context, Type seedData)
    {
      var configuration = resolver.GetService<IConfiguration>();
      var scanAssemblyPattern = configuration.GetSection("EfCore")["FullyQualifiedPrefix"];
      var seeders = scanAssemblyPattern.ResolveModularGenericTypes(seedData, context.GetType());

      if (seeders == null) return;

      foreach (var seeder in seeders)
      {
        var seedInstance = Activator.CreateInstance(seeder, configuration);
        var method = seeder.GetMethod("SeedAsync");
        ((Task)method.Invoke(seedInstance, new object[] {context})).Wait();
      }
    }

    /// <summary>
    ///   This function will open up the door to make the Setup page
    ///   Because we can call to this function for provision new database
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="seeder"></param>
    /// <returns></returns>
    public static IServiceProvider MigrateDbContext<TDbContext>(
      this IServiceProvider serviceProvider,
      Action<TDbContext, IServiceProvider> seeder)
      where TDbContext : DbContext
    {
      var logger = serviceProvider.GetRequiredService<ILogger<TDbContext>>();
      var context = serviceProvider.GetRequiredService<TDbContext>();

      logger.LogInformation($"[VND] Migrating database associated with {typeof(TDbContext).FullName} context.");
      context.Database.OpenConnection();
      if (!context.AllMigrationsApplied()) context.Database.Migrate();

      logger.LogInformation($"[VND] Start to seed data for {typeof(TDbContext).FullName} context.");
      seeder(context, serviceProvider);

      logger.LogInformation($"[VND] Migrated database associated with {typeof(TDbContext).FullName} context.");
      return serviceProvider;
    }
  }
}
