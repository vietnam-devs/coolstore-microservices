using Microsoft.EntityFrameworkCore;

namespace VND.FW.Infrastructure.EfCore.Db
{
  public interface IExtendDbContextOptionsBuilder
  {
    DbContextOptionsBuilder Extend(
        DbContextOptionsBuilder optionsBuilder,
        IDatabaseConnectionStringFactory connectionStringFactory,
        string assemblyName);
  }
}
