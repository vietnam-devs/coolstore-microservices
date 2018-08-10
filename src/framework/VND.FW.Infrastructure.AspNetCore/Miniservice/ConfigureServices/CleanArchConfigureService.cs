using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice.ConfigureServices
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
