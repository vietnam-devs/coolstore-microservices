// Reference at https://thenewstack.io/miniservices-a-realistic-alternative-to-microservices

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice
{
  public static class MiniServiceExtensions
  {
    public static IServiceCollection AddMiniService<TDbContext>(this IServiceCollection services)
      where TDbContext : DbContext
    {
      services.ScanAndRegisterServices<IDbConfigureServices>();
      services.ScanAndRegisterServices<IBasicConfigureServices>();

      var svcProvider = services.BuildServiceProvider();
      var dbConfigureSvcs = svcProvider.GetServicesByPriority<IDbConfigureServices>();
      foreach (var configureSvc in dbConfigureSvcs)
      {
        configureSvc.Configure<TDbContext>(services);
      }

      var configureSvcs = svcProvider.GetServicesByPriority<IBasicConfigureServices>();
      foreach (var configureSvc in configureSvcs)
      {
        configureSvc.Configure(services);
      }

      return services;
    }

    public static IApplicationBuilder UseMiniService(this IApplicationBuilder app)
    {
      var appSvc = app.ApplicationServices;
      var configureApps = appSvc.GetServicesByPriority<IConfigureApplication>();
      foreach (var configureApp in configureApps)
      {
        configureApp.Configure(app);
      }
      return app;
    }
  }
}
