
using System;
using System.Threading.Tasks;
using Google.Protobuf;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloudNativeKit.Infrastructure.Bus.Redis
{
    public class DispatchedEventBus : IDispatchedEventBus
    {
        private readonly ILogger<DispatchedEventBus> _logger;
        private readonly RedisStore _redisStore;
        private readonly IServiceProvider _serviceProvider;

        public DispatchedEventBus(RedisStore redisStore, IServiceProvider serviceProvider, ILoggerFactory factory)
        {
            _redisStore = redisStore;
            _serviceProvider = serviceProvider;
            _logger = factory.CreateLogger<DispatchedEventBus>();
        }

        public async Task PublishAsync<TMessage>(TMessage msg, params string[] channels)
            where TMessage : IMessage<TMessage>
        {
            var redis = _redisStore.RedisCache;
            var pub = redis.Multiplexer.GetSubscriber();

            foreach (var channel in channels)
            {
                _logger.LogInformation($"{channel}: Publishing the message...");
                await pub.PublishAsync(channel, msg.ToByteString().ToByteArray());
            }
        }

        public async Task SubscribeAsync<TMessage>(params string[] channels) where TMessage : IMessage<TMessage>, new()
        {
            var redis = _redisStore.RedisCache;
            var sub = redis.Multiplexer.GetSubscriber();

            foreach (var channel in channels)
                await sub.SubscribeAsync(channel, async (_, message) =>
                {
                    _logger.LogInformation($"{channel}: Subscribe to ${nameof(message)} message.");
                    var msg = (TMessage)Activator.CreateInstance(typeof(TMessage));
                    msg.MergeFrom(message);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Publish(new MessageEnvelope<TMessage>(msg));
                    }
                });
        }
    }
}
