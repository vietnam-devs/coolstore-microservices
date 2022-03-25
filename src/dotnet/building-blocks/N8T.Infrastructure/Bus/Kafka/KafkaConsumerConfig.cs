using System;
using System.Threading.Tasks;
using Avro.Specific;
using Confluent.Kafka;
using Confluent.SchemaRegistry;

namespace N8T.Infrastructure.Bus.Kafka
{
    public class KafkaConsumerConfig : ConsumerConfig
    {
        public const string Name = "Kafka";
        public string Topic { get; set; }

        public string SchemaRegistryUrl { get; set; } = "http://localhost:8081";

        public Func<string, byte[], ISchemaRegistryClient, Task<ISpecificRecord>> EventResolver { get; set; }

        public KafkaConsumerConfig()
        {
            AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
            EnableAutoOffsetStore = false;
        }
    }
}
