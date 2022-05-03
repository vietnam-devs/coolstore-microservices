using System.Threading.Tasks;
using Avro.Generic;
using Avro.Specific;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace N8T.Infrastructure.SchemaRegistry
{
    public static class SchemaRegistryPreCache
    {
        public static async Task PreCacheEvent<TEvent>(ISchemaRegistryClient schemaRegistryClient,
            string topicName = "demo")
            where TEvent : ISpecificRecord, new()
        {
            var @event = new TEvent();
            var serializer = GetSchemaRegistryConfig<TEvent>(schemaRegistryClient);
            var context = new SerializationContext(MessageComponentType.Value, topicName);
            _ = await serializer.SerializeAsync(@event, context);
        }

        internal static AvroSerializer<TEvent> GetSchemaRegistryConfig<TEvent>(
            ISchemaRegistryClient schemaRegistryClient)
            where TEvent : ISpecificRecord
        {
            var serializerConfig = new AvroSerializerConfig
            {
                // ksql only runs with TopicName,
                // see https://docs.confluent.io/platform/current/schema-registry/serdes-develop/index.html#sr-schemas-subject-name-strategy
                SubjectNameStrategy = SubjectNameStrategy.Topic
            };

            var serializer = new AvroSerializer<TEvent>(schemaRegistryClient, serializerConfig);
            return serializer;
        }
    }

    public static class Extensions
    {
        public static IServiceCollection AddSchemeRegistry(this IServiceCollection services, IConfiguration config)
        {
            var registryUrl = config.GetValue("Kafka:SchemaRegistryUrl", "http://localhost:8081");
            services.AddSingleton<ISchemaRegistryClient>(x => new CachedSchemaRegistryClient(
                new SchemaRegistryConfig { Url = registryUrl }));
            return services;
        }

        public static async Task<byte[]> SerializeAsync<TEvent>(this TEvent @event,
            ISchemaRegistryClient schemaRegistryClient, string topicName = "demo")
            where TEvent : ISpecificRecord
        {
            var serializer = SchemaRegistryPreCache.GetSchemaRegistryConfig<TEvent>(schemaRegistryClient);
            var context = new SerializationContext(MessageComponentType.Value, topicName);
            var eventBytes = await serializer.SerializeAsync(@event, context);
            return eventBytes;
        }

        public static async Task<TEvent> DeserializeAsync<TEvent>(this byte[] eventBytes,
            ISchemaRegistryClient schemaRegistryClient, string topicName = "demo")
            where TEvent : ISpecificRecord
        {
            var context = new SerializationContext(MessageComponentType.Value, topicName);
            var testDeserializer = new AvroDeserializer<TEvent>(schemaRegistryClient);
            var @event = await testDeserializer.DeserializeAsync(eventBytes, false, context);
            return @event;
        }

        public static async Task<byte[]> SerializeAsync(this GenericRecord genericRecord,
            ISchemaRegistryClient schemaRegistryClient)
        {
            var serializer = new AvroSerializer<GenericRecord>(schemaRegistryClient);
            var context = new SerializationContext();
            var eventBytes = await serializer.SerializeAsync(genericRecord, context);
            return eventBytes;
        }

        public static async Task<TEvent> DeserializeAsync<TEvent>(this byte[] eventBytes,
            ISchemaRegistryClient schemaRegistryClient)
            where TEvent : ISpecificRecord
        {
            var context = new SerializationContext();
            var testDeserializer = new AvroDeserializer<TEvent>(schemaRegistryClient);
            var @event = await testDeserializer.DeserializeAsync(eventBytes, false, context);
            return @event;
        }
    }
}
