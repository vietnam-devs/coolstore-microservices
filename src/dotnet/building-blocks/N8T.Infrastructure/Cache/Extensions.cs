using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N8T.Infrastructure.Cache.Redis;
using N8T.Infrastructure.Cache.Redis.Internals;
using StackExchange.Redis;

namespace N8T.Infrastructure.Cache
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomRedisCache(this IServiceCollection services, IConfiguration config,
            Action<RedisCacheOptions> setupAction = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (services.Contains(ServiceDescriptor.Singleton<IRedisCacheService, RedisCacheService>()))
            {
                return services;
            }

            var redisOptions = new RedisCacheOptions();
            var redisSection = config.GetSection("Redis");

            redisSection.Bind(redisOptions);
            services.Configure<RedisCacheOptions>(redisSection);
            setupAction?.Invoke(redisOptions);

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = config[redisOptions.Prefix];
                options.ConfigurationOptions = GetRedisConfigurationOptions(redisOptions);
            });

            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            return services;
        }

        private static ConfigurationOptions GetRedisConfigurationOptions(RedisCacheOptions redisOptions)
        {
            var configurationOptions = new ConfigurationOptions
            {
                ConnectTimeout = redisOptions.ConnectTimeout,
                SyncTimeout = redisOptions.SyncTimeout,
                ConnectRetry = redisOptions.ConnectRetry,
                AbortOnConnectFail = redisOptions.AbortOnConnectFail,
                ReconnectRetryPolicy = new ExponentialRetry(redisOptions.DeltaBackoffMiliseconds),
                KeepAlive = 5,
                Ssl = redisOptions.Ssl
            };

            if (!string.IsNullOrWhiteSpace(redisOptions.Password))
            {
                configurationOptions.Password = redisOptions.Password;
            }

            var endpoints = redisOptions.Url.Split(',');
            foreach (var endpoint in endpoints)
            {
                configurationOptions.EndPoints.Add(endpoint);
            }

            return configurationOptions;
        }
    }
}