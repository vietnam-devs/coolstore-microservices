using Microsoft.EntityFrameworkCore;
using NetCoreKit.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Inventory.v1.Db
{
  public class InventoryDbContext : AppDbContext
  {
    public InventoryDbContext(DbContextOptions options) : base(options)
    {
    }
  }
}
