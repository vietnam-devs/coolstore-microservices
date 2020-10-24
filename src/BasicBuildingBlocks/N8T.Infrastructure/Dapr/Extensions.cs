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
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            services.AddDaprClient(client =>
            {
                client.UseJsonSerializationOptions(options);
            });

            return services;
        }
    }
}
