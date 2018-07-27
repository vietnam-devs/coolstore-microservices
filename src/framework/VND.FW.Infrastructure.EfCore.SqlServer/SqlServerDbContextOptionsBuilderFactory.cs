using Microsoft.EntityFrameworkCore;
using System;
using VND.FW.Infrastructure.EfCore.Db;

namespace VND.FW.Infrastructure.EfCore.SqlServer
{
  public sealed class SqlServerDbContextOptionsBuilderFactory : IExtendDbContextOptionsBuilder
  {
    public DbContextOptionsBuilder Extend(DbContextOptionsBuilder optionsBuilder, IDatabaseConnectionStringFactory connectionStringFactory, string assemblyName)
    {
      return optionsBuilder.UseSqlServer(
         connectionStringFactory.Create(),
         sqlOptions =>
         {
           sqlOptions.MigrationsAssembly(assemblyName);
           /*sqlOptions.EnableRetryOnFailure(
                     maxRetryCount: 15,
                     maxRetryDelay: TimeSpan.FromSeconds(30),
                     errorNumbersToAdd: null);*/
         })
         .EnableSensitiveDataLogging();
    }
  }
}
