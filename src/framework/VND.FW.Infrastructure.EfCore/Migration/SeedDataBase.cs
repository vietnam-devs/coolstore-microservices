using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace VND.FW.Infrastructure.EfCore.Migration
{
  public abstract class SeedDataBase<TDbContext>
        : ISeedData<TDbContext>, IAuthConfigSeedData<TDbContext>
        where TDbContext : DbContext
  {
    protected IConfiguration Configuration { get; }

    protected SeedDataBase(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public abstract Task SeedAsync(TDbContext context);
  }
}
