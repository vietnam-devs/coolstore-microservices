using Microsoft.EntityFrameworkCore;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db;

namespace CloudNativeKit.Infrastructure.DataPersistence.InMemory
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
