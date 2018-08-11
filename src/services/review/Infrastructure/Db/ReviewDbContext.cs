using Microsoft.EntityFrameworkCore;
using NetCoreKit.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Review.Infrastructure.Db
{
  public class ReviewDbContext : ApplicationDbContext
  {
    public ReviewDbContext(DbContextOptions options) : base(options)
    {
    }
  }
}
