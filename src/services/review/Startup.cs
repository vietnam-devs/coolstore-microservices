using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.EfCore.MySql;
using NetCoreKit.Template.EfCore;
using VND.CoolStore.Services.Review.Infrastructure.Db;

namespace VND.CoolStore.Services.Review
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddEfCoreTemplate<ReviewDbContext>(
        svc => svc.AddEfCoreMySqlDb()
      );
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseEfCoreTemplate();
    }
  }
}
