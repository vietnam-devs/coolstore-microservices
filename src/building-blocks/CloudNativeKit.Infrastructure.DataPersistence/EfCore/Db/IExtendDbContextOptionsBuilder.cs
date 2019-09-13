using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db
{
    public interface IExtendDbContextOptionsBuilder
    {
        DbContextOptionsBuilder Extend(DbContextOptionsBuilder optionsBuilder, IDbConnStringFactory connectionStringFactory, string assemblyName);
    }
}
