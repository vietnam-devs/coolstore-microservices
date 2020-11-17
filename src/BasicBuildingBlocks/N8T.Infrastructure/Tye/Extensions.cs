using System;
using Microsoft.Extensions.Configuration;

namespace N8T.Infrastructure.Tye
{
    public static class Extensions
    {
        public static Uri GetGraphQlUriFor(this IConfiguration config, string appId)
        {
            var appOptions = config.GetOptions<AppOptions>("app");

            var clientUrl = !appOptions.NoTye.Enabled
                ? config.GetServiceUri(appId)?.ToString()
                : config.GetValue<string>($"app:noTye:services:{appId}:url");

            return new Uri($"{clientUrl?.TrimEnd('/')}/graphql");
        }

        public static Uri GetRestUriFor(this IConfiguration config, string appId)
        {
            var appOptions = config.GetOptions<AppOptions>("app");

            var clientUrl = !appOptions.NoTye.Enabled
                ? config.GetServiceUri(appId)?.ToString()
                : config.GetValue<string>($"app:noTye:services:{appId}:url");

            return new Uri(clientUrl?.TrimEnd('/')!);
        }

        public static Uri GetGrpcUriFor(this IConfiguration config, string appId)
        {
            var appOptions = config.GetOptions<AppOptions>("app");

            string clientUrl;
            if (!appOptions.NoTye.Enabled)
            {
                clientUrl = config
                    .GetServiceUri(appId, "https")
                    ?.ToString()
                    .Replace("https", "http") /* hack: ssl termination */;
            }
            else
            {
                clientUrl = config.GetValue<string>($"app:noTye:services:{appId}:grpcUrl");
            }

            return new Uri(clientUrl!);
        }

        public static bool IsRunOnTye(this IConfiguration config, string serviceName)
        {
            return config.GetServiceUri(serviceName) is not null;
        }
    }
}
