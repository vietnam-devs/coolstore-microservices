using System;
using System.Text.Json;
using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using RestEase;
using RestEase.HttpClientFactory;

namespace N8T.Infrastructure.ServiceInvocation.Dapr
{
    public static class Extensions
    {
        public static IServiceCollection AddDaprClient(this IServiceCollection services)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };

            services.AddSingleton(options);

            services.AddDaprClient(client =>
            {
                client.UseJsonSerializationOptions(options);
            });

            return services;
        }

        public static IServiceCollection AddRestClient(this IServiceCollection services,
            Type httpClientApi, string appName = "localhost", int appPort = 5000)
        {
            var appUri = $"http://{appName}:{appPort}";

            services.AddScoped<InvocationHandler>();
            services.AddRestEaseClient(httpClientApi, appUri, client =>
            {
                client.RequestPathParamSerializer = new StringEnumRequestPathParamSerializer();
            }).AddHttpMessageHandler<InvocationHandler>();

            return services;
        }
    }
}
