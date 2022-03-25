using System;
using System.Reflection;
using System.Text.Json.Serialization;
using N8T.Core.Domain;
using Newtonsoft.Json;

namespace N8T.Infrastructure.TxOutbox
{
    public class OutboxEntity
    {
        [JsonInclude]
        public Guid Id { get; private set; }

        [JsonInclude]
        public System.DateTime OccurredOn { get; private set; }

        [JsonInclude]
        public string Type { get; private set; }

        [JsonInclude]
        public string Data { get; private set; }

        public OutboxEntity()
        {
            // only for System.Text.Json to deserialized data
        }

        public OutboxEntity(Guid id, System.DateTime occurredOn, IDomainEvent @event)
        {
            Id = id.Equals(Guid.Empty) ? Guid.NewGuid() : id;
            OccurredOn = occurredOn;
            Type = @event.GetType().FullName;
            Data = JsonConvert.SerializeObject(@event);
        }

        public virtual IDomainEvent RecreateMessage(Assembly assembly) => (IDomainEvent)JsonConvert.DeserializeObject(Data, assembly.GetType(Type)!);
    }
}
