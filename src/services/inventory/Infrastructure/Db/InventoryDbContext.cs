using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VND.FW.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Inventory.Infrastructure.Db
{
  public class InventoryDbContext : ApplicationDbContext<InventoryDbContext>
  {
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IConfiguration configuration)
      : base(options, configuration)
    {
    }
  }
}
