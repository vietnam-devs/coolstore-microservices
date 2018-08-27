using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.AspNetCore.Miniservice;
using NetCoreKit.Infrastructure.EfCore.MySql;
using VND.CoolStore.Services.Inventory.Infrastructure.Db;

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
          svc.AddExternalSystemHealthChecks();
        },
        (_, __) => { },
        () => new Dictionary<string, object>
        {
          [Constants.ClaimToScopeMap] = new Dictionary<string, string>
          {
            ["access_inventory_api"] = "inventory_api_scope"
          },
          [Constants.Scopes] = new Dictionary<string, string>
          {
            ["inventory_api_scope"] = "Inventory APIs"
          },
          [Constants.Audience] = "api"
        }
      );
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
