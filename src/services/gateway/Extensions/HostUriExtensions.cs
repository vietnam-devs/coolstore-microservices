using Microsoft.Extensions.Configuration;
using System;

namespace VND.CoolStore.Services.ApiGateway.Extensions
{
		internal static class HostUriExtensions
		{
				public static string GetInternalAuthHostUri(this IConfiguration config)
				{
						var serviceHost = Environment.GetEnvironmentVariable("IDP_SERVICE_SERVICE_HOST");
						var servicePort = Environment.GetEnvironmentVariable("IDP_SERVICE_SERVICE_PORT");
						return $"http://{serviceHost}:{servicePort}";
				}

				public static string GetExternalAuthHostUri(this IConfiguration config)
				{
						return config.GetSection("HostSettings")?.GetValue<string>("ExternalAuthHostUri");
				}

				public static string GetBasePath(this IConfiguration config)
				{
						return config.GetSection("HostSettings")?.GetValue<string>("BasePath");
				}

				public static string GetExternalCurrentHostUri(this IConfiguration config)
				{
						return config.GetSection("HostSettings")?.GetValue<string>("ExternalCurrentHostUri");
				}
		}
}
