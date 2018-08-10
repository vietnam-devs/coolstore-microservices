using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice
{
  public static class ServiceProviderExtensions
  {
    public static IOrderedEnumerable<TConfigureService> GetServicesByPriority<TConfigureService>(
      this IServiceProvider svcProvider)
      where TConfigureService : IPriorityConfigure
    {
      return svcProvider
        .GetServices<TConfigureService>()
        .OrderBy(x => x.Priority);
    }
  }
}
