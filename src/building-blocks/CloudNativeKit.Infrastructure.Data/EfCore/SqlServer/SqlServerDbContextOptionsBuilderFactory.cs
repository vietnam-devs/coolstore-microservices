using System;
using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.Data.EfCore.SqlServer
{
    using CloudNativeKit.Infrastructure.Data.EfCore.Core.Db;

    public sealed class SqlServerDbContextOptionsBuilderFactory : IExtendDbContextOptionsBuilder
    {
        public DbContextOptionsBuilder Extend(DbContextOptionsBuilder optionsBuilder, IDbConnStringFactory connStringFactory, string assemblyName)
        {
            return optionsBuilder.UseSqlServer(
                    connStringFactory.Create(),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(assemblyName);
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    })
                .EnableSensitiveDataLogging();
        }
    }
}
