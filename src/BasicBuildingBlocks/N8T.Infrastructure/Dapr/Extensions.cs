using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace N8T.Infrastructure.Dapr
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomDaprClient(this IServiceCollection services)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true,
            };

            services.AddSingleton(options);

            services.AddDaprClient(client =>
            {
                client.UseJsonSerializationOptions(options);
            });

            return services;
        }
    }
}
