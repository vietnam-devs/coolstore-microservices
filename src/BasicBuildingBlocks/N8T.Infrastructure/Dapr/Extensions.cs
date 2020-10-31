using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace N8T.Infrastructure.Dapr
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomDaprClient(this IServiceCollection services)
        {
            services.AddDaprClient();

            services.AddSingleton(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });

            return services;
        }
    }
}
