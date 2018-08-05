using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using VND.CoolStore.Services.Inventory.Infrastructure.Db;
using VND.CoolStore.Services.Inventory.UseCases.Service;
using VND.CoolStore.Services.Inventory.UseCases.Service.Impl;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Inventory
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreSqlServer();
      services.AddMiniService<InventoryDbContext>(
        typeof(Startup).GetTypeInfo().Assembly,
        o =>
        {
          o.AddPolicy("access_inventory_api", p => p.RequireClaim("scope", "inventory_api_scope"));
        },
        () =>
        {
          return new Dictionary<string, string>
              {
                {"inventory_api_scope", "Inventory APIs"}
              };
        }
      );
      services.AddScoped<IInventoryService, InventoryService>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
