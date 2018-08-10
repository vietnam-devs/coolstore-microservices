using Microsoft.EntityFrameworkCore;

namespace VND.Fw.Infrastructure.EfCore.Db
{
  public interface IExtendDbContextOptionsBuilder
  {
    DbContextOptionsBuilder Extend(
        DbContextOptionsBuilder optionsBuilder,
        IDatabaseConnectionStringFactory connectionStringFactory,
        string assemblyName);
  }
}
