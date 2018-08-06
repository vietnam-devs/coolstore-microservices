using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Db;
using VND.CoolStore.Services.Cart.UseCases.v1.Services;
using VND.CoolStore.Services.Cart.v1.UseCases.Checkout;
using VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart;
using VND.CoolStore.Services.Cart.v1.UseCases.GetCartById;
using VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart;
using VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Cart
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreSqlServer();
      services.AddMiniService<CartDbContext>(
        typeof(Startup).GetTypeInfo().Assembly,
        o =>
        {
          o.AddPolicy("access_cart_api",
            p => p.RequireClaim("scope", "cart_api_scope"));
        },
        () =>
        {
          return new Dictionary<string, string>
          {
            {"cart_api_scope", "Cart APIs"}
          };
        }
      );

      services.AddScoped<NoTaxCaculator>();
      services.AddScoped<TenPercentTaxCalculator>();

      services.AddScoped<GetCartPresenter>();
      services.AddScoped<InsertItemPresenter>();
      services.AddScoped<UpdateItemPresenter>();
      services.AddScoped<DeleteItemPresenter>();
      services.AddScoped<CheckoutPresenter>();
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseMiniService();
    }
  }
}
