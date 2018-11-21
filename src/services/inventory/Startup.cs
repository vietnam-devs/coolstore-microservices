using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.AspNetCore.Miniservice;
using NetCoreKit.Infrastructure.EfCore.MySql;
using VND.CoolStore.Services.Inventory.v1.Infrastructure.Db;

namespace VND.CoolStore.Services.Inventory
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMiniService<InventoryDbContext>(
        svc =>
        {
          svc.AddEfCoreMySqlDb();
        }
      );
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
