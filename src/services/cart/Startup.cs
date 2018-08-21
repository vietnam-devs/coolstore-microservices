using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.AspNetCore.Miniservice;
using NetCoreKit.Infrastructure.EfCore.MySql;
using VND.CoolStore.Services.Cart.Infrastructure.Db;

namespace VND.CoolStore.Services.Cart
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMiniService<CartDbContext>(
        new[] {typeof(Startup)},
        svc =>
        {
          svc.AddEfCoreMySqlDb();
          svc.AddExternalSystemHealthChecks();
        },
        () => new Dictionary<string, object>
        {
          {
            Constants.ClaimToScopeMap, new Dictionary<string, string>
            {
              {"access_cart_api", "cart_api_scope"}
            }
          },
          {
            Constants.Scopes, new Dictionary<string, string>
            {
              {"cart_api_scope", "Cart APIs"}
            }
          },
          {Constants.Audience, "api"}
        }
      );
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
