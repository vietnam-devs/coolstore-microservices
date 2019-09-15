using Microsoft.EntityFrameworkCore;
using CloudNativeKit.Infrastructure.Data.EfCore.Core.Db;

namespace CloudNativeKit.Infrastructure.Data.InMemory
{
    internal class InMemoryDbContextOptionsBuilderFactory : IExtendDbContextOptionsBuilder
    {
        public DbContextOptionsBuilder Extend(
            DbContextOptionsBuilder optionsBuilder,
            IDbConnStringFactory connStringFactory,
            string assemblyName)
        {
            return optionsBuilder.UseInMemoryDatabase("defaultdb");
        }
    }
}
