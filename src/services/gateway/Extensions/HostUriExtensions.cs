using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace VND.CoolStore.Services.ApiGateway.Extensions
{
  internal static class HostUriExtensions
  {
    public static string GetHostUri(this IConfiguration config, IHostingEnvironment env, string groupName)
    {
      if (env.IsDevelopment())
      {
        return config.GetExternalHostUri(groupName);
      }
      return config.GetInternalHostUri(groupName);
    }

    public static string GetInternalHostUri(this IConfiguration config, string groupName)
    {
      IConfigurationSection group =
        config
          .GetSection("Hosts")
          ?.GetSection("Internals")
          ?.GetSection(groupName);

      string serviceHost = $"{Environment.GetEnvironmentVariable(group.GetValue<string>("Host"))}";
      string servicePort = $"{Environment.GetEnvironmentVariable(group.GetValue<string>("Port"))}";

      return $"http://{serviceHost}:{servicePort}";
    }

    public static string GetExternalHostUri(this IConfiguration config, string groupName)
    {
      IConfigurationSection group =
        config
          .GetSection("Hosts")
          ?.GetSection("Externals")
          ?.GetSection(groupName);

      return group.GetValue<string>("Uri");
    }

    public static string GetBasePath(this IConfiguration config)
    {
      return config.GetSection("Hosts")?.GetValue<string>("BasePath");
    }

    public static string GetExternalCurrentHostUri(this IConfiguration config)
    {
      return config.GetSection("Hosts")?.GetSection("Externals")?.GetValue<string>("CurrentUri");
    }
  }
}
