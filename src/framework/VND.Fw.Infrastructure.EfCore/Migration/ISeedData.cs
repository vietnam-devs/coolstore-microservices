using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VND.Fw.Infrastructure.EfCore.Migration
{
  public interface ISeedData<TDbContext>
        where TDbContext : DbContext
  {
    Task SeedAsync(TDbContext context);
  }

  public interface IAuthConfigSeedData<TDbContext> : ISeedData<TDbContext>
      where TDbContext : DbContext
  {
  }
}
