using Microsoft.EntityFrameworkCore;

namespace CloudNativeKit.Infrastructure.Data.EfCore.Core.Db
{
    public interface IExtendDbContextOptionsBuilder
    {
        DbContextOptionsBuilder Extend(DbContextOptionsBuilder optionsBuilder, IDbConnStringFactory connectionStringFactory, string assemblyName);
    }
}
