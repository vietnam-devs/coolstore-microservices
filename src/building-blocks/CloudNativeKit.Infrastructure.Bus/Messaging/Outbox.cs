using System;
using CloudNativeKit.Domain;
using Newtonsoft.Json;
using static CloudNativeKit.Utils.Helpers.DateTimeHelper;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace CloudNativeKit.Infrastructure.Bus.Messaging
{
    public class Outbox : AggregateRootBase<Guid>
    {
        public DateTime OccurredOn { get; private set; }

        public string Type { get; private set; }

        public string Data { get; private set; }

        public DateTime? ProcessedDate { get; private set; }

        private Outbox() { }

        public Outbox(Guid id, DateTime occurredOn, IEvent @event)
        {
            Id = id.Equals(Guid.Empty) ? NewId() : id;
            OccurredOn = occurredOn;
            Type = @event.GetType().FullName;
            Data = JsonConvert.SerializeObject(@event);
        }

        public virtual IEvent RecreateMessage() => (IEvent)JsonConvert.DeserializeObject(Data, System.Type.GetType(Type));

        public Outbox UpdateProcessedDate()
        {
            ProcessedDate = NewDateTime();
            return this;
        }
    }
}
