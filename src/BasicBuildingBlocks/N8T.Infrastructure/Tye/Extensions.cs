using Microsoft.Extensions.Configuration;

namespace N8T.Infrastructure.Tye
{
    public static class Extensions
    {
        public static bool IsRunOnTye(this IConfiguration config)
        {
            var serviceName = config.GetValue<string>("App:Name");
            return config.GetServiceUri(serviceName) is not null;
        }
    }
}
