using Microsoft.EntityFrameworkCore;
using VND.Fw.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Cart.Infrastructure.Db
{
  public class CartDbContext : ApplicationDbContext
  {
    public CartDbContext(DbContextOptions options) : base(options)
    {
    }
  }
}
