using Microsoft.EntityFrameworkCore;
using NetCoreKit.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Inventory.Infrastructure.Db
{
  public class InventoryDbContext : ApplicationDbContext
  {
    public InventoryDbContext(DbContextOptions options) : base(options)
    {
    }
  }
}
