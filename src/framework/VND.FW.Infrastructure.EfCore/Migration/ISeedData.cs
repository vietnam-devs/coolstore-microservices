using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace VND.FW.Infrastructure.EfCore.Migration
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
