using System;
using System.Threading;
using System.Threading.Tasks;
using Avro.Generic;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N8T.Infrastructure.SchemaRegistry;

namespace N8T.Infrastructure.Bus.Kafka
{
    public class BackGroundKafkaConsumer : BackgroundService
    {
        private readonly KafkaConsumerConfig _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<BackGroundKafkaConsumer> _logger;

        public BackGroundKafkaConsumer(IOptions<KafkaConsumerConfig> config,
            IServiceScopeFactory serviceScopeFactory, ILogger<BackGroundKafkaConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _config = config.Value;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(() => ConsumeTopic(stoppingToken),
                stoppingToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        private async Task ConsumeTopic(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                // Note: you can specify more than one schema registry url using the
                // schema.registry.url property for redundancy (comma separated list).
                // The property name is not plural to follow the convention set by
                // the Java implementation.
                Url = _config.SchemaRegistryUrl
            };

            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var consumer = new ConsumerBuilder<string, GenericRecord>(_config)
                .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) => _logger.LogInformation($"Statistics: {json}"))
                .SetValueDeserializer(new AvroDeserializer<GenericRecord>(schemaRegistry).AsSyncOverAsync())
                .Build();

            consumer.Subscribe(_config.Topic);

            try
            {
                while (stoppingToken.IsCancellationRequested == false)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);

                        if (result is null)
                            continue;

                        var fullSchemaName = result.Message.Value.Schema.SchemaName.Fullname;
                        var genericRecord = result.Message.Value;
                        var bytes = await genericRecord.SerializeAsync(schemaRegistry);
                        var @event = await _config.EventResolver?.Invoke(fullSchemaName, bytes, schemaRegistry)!;

                        _logger.LogInformation($"Received {result.Message?.Key!}-{result.Message?.Value?.GetType().FullName!} message.");
                        if (@event is INotification)
                        {
                            await mediator.Publish(@event, stoppingToken);
                            _logger.LogInformation($"Dispatched {@event.GetType()?.FullName} event to internal handler.");
                        }

                        consumer.Commit(result);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogInformation(ex.Message);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // commit final offsets and leave the group.
                consumer.Close();
            }
        }
    }
}
