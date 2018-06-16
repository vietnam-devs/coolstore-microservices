using Microsoft.EntityFrameworkCore;

namespace VND.FW.Infrastructure.EfCore
{
    public interface IExtendDbContextOptionsBuilder
    {
        DbContextOptionsBuilder Extend(
            DbContextOptionsBuilder optionsBuilder,
            IDatabaseConnectionStringFactory connectionStringFactory, 
            string assemblyName);
    }
}
