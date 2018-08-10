using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection ScanAndRegisterServices<TConfigureService>(this IServiceCollection services)
    {
      var svcProvider = services.BuildServiceProvider();
      var serviceParams = svcProvider.GetRequiredService<ServiceParams>();
      var assemblies = serviceParams["assemblies"] as HashSet<Assembly>;

      return services.Scan(
        scanner => scanner
          .FromAssemblies(assemblies)
          .AddClasses(x => x.AssignableTo<TConfigureService>())
          .AsImplementedInterfaces()
          .WithScopedLifetime());
    }
  }
}
