using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.EfCore.MySql;
using NetCoreKit.Template.EfCore;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Db;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;

namespace VND.CoolStore.Services.Cart
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreTemplate<CartDbContext>(
        svc =>
        {
          svc.AddEfCoreMySqlDb();
        },
        (svc, _) =>
        {
          svc.AddScoped<ICatalogGateway, CatalogGateway>();
          svc.AddScoped<IPromoGateway, PromoGateway>();
          svc.AddScoped<IShippingGateway, ShippingGateway>();
        }
      );
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseEfCoreTemplate();
    }
  }
}
