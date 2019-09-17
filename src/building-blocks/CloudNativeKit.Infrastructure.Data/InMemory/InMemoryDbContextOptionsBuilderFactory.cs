using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.Data.InMemory
{
    using CloudNativeKit.Infrastructure.Data.EfCore.Core.Db;

    internal class InMemoryDbContextOptionsBuilderFactory : IExtendDbContextOptionsBuilder
    {
        public DbContextOptionsBuilder Extend(DbContextOptionsBuilder optionsBuilder, IDbConnStringFactory connStringFactory, string assemblyName)
        {
            return optionsBuilder.UseInMemoryDatabase("defaultdb");
        }
    }
}
