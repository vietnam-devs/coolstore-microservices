using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VND.FW.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Cart.Infrastructure.Db
{
  public class CartDbContext : ApplicationDbContext
  {
    public CartDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
      : base(options, configuration)
    {
    }
  }
}
