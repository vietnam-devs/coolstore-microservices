using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VND.FW.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Cart.Infrastructure.Db
{
  public class CartDbContext : ApplicationDbContext<CartDbContext>
  {
    public CartDbContext(DbContextOptions<CartDbContext> options, IConfiguration configuration)
      : base(options, configuration)
    {
    }
  }
}
