using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VND.CoolStore.Services.Cart.UseCases.Services;
using VND.CoolStore.Services.Cart.UseCases.Services.Impl;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Cart
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreSqlServer();
      services.AddScoped<ICartService, CartService>();
      services.AddMiniService(typeof(Startup).GetTypeInfo().Assembly);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      app.UseMiniService();
    }
  }
}
