using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N8T.Core.Domain;
using N8T.Infrastructure.TxOutbox.Dapr;
using N8T.Infrastructure.TxOutbox.Dapr.Internal;
using N8T.Infrastructure.TxOutbox.InMemory;

namespace N8T.Infrastructure.TxOutbox
{
    public class TxOutboxConstants
    {
        public const string InMemory = "inmem";
        public const string Dapr = "dapr";
    }

    public static class Extensions
    {
        public static IServiceCollection AddTransactionalOutbox(this IServiceCollection services, IConfiguration config,
            string provider = TxOutboxConstants.InMemory)
        {
            switch (provider)
            {
                case TxOutboxConstants.InMemory:
                    {
                        services.AddSingleton<IEventStorage, EventStorage>();
                        services.AddScoped<INotificationHandler<EventWrapper>, LocalDispatchedHandler>();
                        services.AddScoped<ITxOutboxProcessor, TxOutboxProcessor>();
                        break;
                    }
                case TxOutboxConstants.Dapr:
                    {
                        services.Configure<DaprTxOutboxOptions>(config.GetSection(DaprTxOutboxOptions.Name));
                        services.AddScoped<INotificationHandler<EventWrapper>, DaprLocalDispatchedHandler>();
                        services.AddScoped<ITxOutboxProcessor, DaprTxOutboxProcessor>();
                        break;
                    }
            }

            return services;
        }
    }
}
