using Microsoft.EntityFrameworkCore;
using VND.Fw.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Review.Infrastructure.Db
{
  public class ReviewDbContext : ApplicationDbContext
  {
    public ReviewDbContext(DbContextOptions options) : base(options)
    {
    }
  }
}
