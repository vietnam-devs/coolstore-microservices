using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.AspNetCore.Miniservice;
using NetCoreKit.Infrastructure.EfCore.SqlServer;
using VND.CoolStore.Services.Cart.Infrastructure.Db;
using VND.CoolStore.Services.Cart.v1.Services;
using VND.CoolStore.Services.Cart.v2.Services;

namespace VND.CoolStore.Services.Cart
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
        {"access_cart_api", "cart_api_scope"}
      };

      var scopes = new Dictionary<string, string>
      {
        {"cart_api_scope", "Cart APIs"}
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
      services.AddScoped<NoTaxCaculator>();
      services.AddScoped<TenPercentTaxCalculator>();
      services.AddMiniService<CartDbContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
