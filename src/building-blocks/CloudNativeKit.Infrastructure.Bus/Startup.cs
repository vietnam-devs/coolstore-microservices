using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CloudNativeKit.Domain;
using Microsoft.Extensions.Configuration;

namespace CloudNativeKit.Infrastructure.Bus
{
    public static class Startup
    {
        public static IServiceCollection AddDomainEventBus(this IServiceCollection services)
        {
            services.Replace(ServiceDescriptor.Singleton<IDomainEventDispatcher, InMemory.DomainEventDispatcher>());
            return services;
        }

        public static IServiceCollection AddRedisBus(this IServiceCollection services)
        {
            var resolver = services.BuildServiceProvider();
            using (var scope = resolver.CreateScope())
            {
                var config = scope.ServiceProvider.GetService<IConfiguration>();
                var redisOptions = config.GetSection("Redis");

                services.Configure<Redis.RedisOptions>(o =>
                {
                    o.Fqdn = redisOptions.GetValue<string>("FQDN");
                    o.Password = redisOptions.GetValue<string>("Password");
                });

                services.AddSingleton<Redis.RedisStore>();
                services.AddSingleton<IDispatchedEventBus, Redis.DispatchedEventBus>();
                return services;
            }
        }
    }
}
