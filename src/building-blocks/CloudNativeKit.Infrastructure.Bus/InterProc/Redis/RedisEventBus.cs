using System;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudNativeKit.Infrastructure.Bus.InterProc.Redis
{
    public class RedisEventBus : IMessageBus
    {
        private readonly ILogger<RedisEventBus> _logger;
        private readonly RedisStore _redisStore;
        private readonly IServiceProvider _serviceProvider;

        public RedisEventBus(RedisStore redisStore, IServiceProvider serviceProvider, ILoggerFactory factory)
        {
            _redisStore = redisStore;
            _serviceProvider = serviceProvider;
            _logger = factory.CreateLogger<RedisEventBus>();
        }

        public async Task PublishAsync<TMessage>(TMessage msg, params string[] channels)
            where TMessage : IIntegrationEvent
        {
            var redis = _redisStore.RedisCache;
            var pub = redis.Multiplexer.GetSubscriber();

            foreach (var channel in channels)
            {
                _logger.LogInformation($"{channel}: Publishing the message with content {JsonConvert.SerializeObject(msg)}");
                await pub.PublishAsync(channel, msg.ToByteArray());
            }
        }

        public async Task SubscribeAsync<TMessage>(params string[] channels) where TMessage : IIntegrationEvent, new()
        {
            var redis = _redisStore.RedisCache;
            var sub = redis.Multiplexer.GetSubscriber();

            foreach (var channel in channels)
                await sub.SubscribeAsync(channel, async (_, message) =>
                {
                    _logger.LogInformation($"{channel}: Subscribe to ${nameof(message)} message and the content is {JsonConvert.SerializeObject(message)}.");
                    var msg = Utils.ObjectFactory<TMessage>.CreateInstance();
                    using var scope = _serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Publish(new MessageEnvelope<TMessage>(msg));
                });
        }
    }
}
