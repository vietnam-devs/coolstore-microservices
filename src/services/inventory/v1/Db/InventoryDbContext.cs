using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.EfCore.Db;

namespace VND.CoolStore.Services.Inventory.v1.Db
{
    public class InventoryDbContext : AppDbContext
    {
        public InventoryDbContext(DbContextOptions options, IConfiguration config, IDomainEventDispatcher eventBus = null)
            : base(options, config, eventBus)
        {
        }
    }
}
