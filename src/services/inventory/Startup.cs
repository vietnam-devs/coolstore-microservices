using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.EfCore.MySql;
using NetCoreKit.Template.EfCore;
using VND.CoolStore.Services.Inventory.v1.Db;

namespace VND.CoolStore.Services.Inventory
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreTemplate<InventoryDbContext>(
        svc =>
        {
          svc.AddEfCoreMySqlDb();
        }
      );
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseEfCoreTemplate();
    }
  }
}
