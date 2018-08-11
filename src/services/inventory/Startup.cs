using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.AspNetCore.Miniservice;
using NetCoreKit.Infrastructure.EfCore.SqlServer;
using VND.CoolStore.Services.Inventory.Infrastructure.Db;

namespace VND.CoolStore.Services.Inventory
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      var assemblies = new HashSet<Assembly>
      {
        typeof(Startup).GetTypeInfo().Assembly,
        typeof(MiniServiceExtensions).GetTypeInfo().Assembly
      };

      var claimToScopeMap = new Dictionary<string, string>
      {
        {"access_inventory_api", "inventory_api_scope"}
      };

      var scopes = new Dictionary<string, string>
      {
        {"inventory_api_scope", "Inventory APIs"}
      };

      var serviceParams = new ServiceParams
      {
        {"assemblies", assemblies},
        {"audience", "api"},
        {"claimToScopeMap", claimToScopeMap},
        {"scopes", scopes}
      };

      services.AddScoped(sp => serviceParams);
      services.AddEfCoreSqlServer();
      services.AddMiniService<InventoryDbContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
