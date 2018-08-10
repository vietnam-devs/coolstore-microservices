using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice.ConfigureServices
{
  public class CleanArchConfigureService : IBasicConfigureServices
  {
    public int Priority { get; } = 300;

    public void Configure(IServiceCollection services)
    {
      var svcProvider = services.BuildServiceProvider();
      var serviceParams = svcProvider.GetRequiredService<ServiceParams>();
      var assemblies = serviceParams["assemblies"] as HashSet<Assembly>;
      assemblies.Add(typeof(MiniServiceExtensions).Assembly);

      services.AddMediatR(assemblies.ToArray());
      services.Scan(
        scanner => scanner
          .FromAssemblies(assemblies.ToArray())
          .AddClasses()
          .AsImplementedInterfaces());
    }
  }
}
