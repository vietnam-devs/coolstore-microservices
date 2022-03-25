using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N8T.Infrastructure.Bus.Dapr;
using N8T.Infrastructure.Bus.Dapr.Internal;
using N8T.Infrastructure.Bus.Kafka;
using N8T.Infrastructure.SchemaRegistry;

namespace N8T.Infrastructure.Bus
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services,
            IConfiguration config,
            string messageBrokerType = "dapr")
        {
            switch (messageBrokerType)
            {
                case "dapr":
                    services.Configure<DaprEventBusOptions>(config.GetSection(DaprEventBusOptions.Name));
                    services.AddScoped<IEventBus, DaprEventBus>();
                    break;
            }

            return services;
        }

        public static IServiceCollection AddKafkaConsumer(this IServiceCollection services,
            Action<KafkaConsumerConfig> configAction)
        {
            services.AddHostedService<BackGroundKafkaConsumer>();

            services.AddOptions<KafkaConsumerConfig>()
                .BindConfiguration(KafkaConsumerConfig.Name)
                .Configure(configAction);

            return services;
        }
    }
}
