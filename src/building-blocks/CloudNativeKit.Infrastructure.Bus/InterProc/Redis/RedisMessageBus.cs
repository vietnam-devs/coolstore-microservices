using System;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using Google.Protobuf;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudNativeKit.Infrastructure.Bus.InterProc.Redis
{
    public class RedisMessageBus : IMessageBus
    {
        private readonly ILogger<RedisMessageBus> _logger;
        private readonly RedisStore _redisStore;
        private readonly IServiceProvider _serviceProvider;

        public RedisMessageBus(RedisStore redisStore, IServiceProvider serviceProvider, ILoggerFactory factory)
        {
            _redisStore = redisStore;
            _serviceProvider = serviceProvider;
            _logger = factory.CreateLogger<RedisMessageBus>();
        }

        public async Task PublishAsync<TMessage>(TMessage msg, params string[] channels)
            where TMessage : IMessage<TMessage>, IIntegrationEvent
        {
            var redis = _redisStore.RedisCache;
            var pub = redis.Multiplexer.GetSubscriber();

            foreach (var channel in channels)
            {
                _logger.LogInformation($"{channel}: Publishing the message with content is {JsonConvert.SerializeObject(msg)}.");
                await pub.PublishAsync(channel, msg.ToByteString().ToByteArray());
            }
        }

        public async Task SubscribeAsync<TMessage>(params string[] channels)
            where TMessage : IMessage<TMessage>, IIntegrationEvent, new()
        {
            var redis = _redisStore.RedisCache;
            var sub = redis.Multiplexer.GetSubscriber();

            foreach (var channel in channels)
                await sub.SubscribeAsync(channel, async (_, message) =>
                {
                    _logger.LogInformation($"{channel}: Subscribe the message with the content is {JsonConvert.SerializeObject(message)}.");
                    var msg = Utils.ObjectFactory<TMessage>.CreateInstance();
                    msg.MergeFrom(message);
                    using var scope = _serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Publish(new MessageEnvelope<TMessage>(msg));
                });
        }
    }
}
