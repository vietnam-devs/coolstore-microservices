using System;
using CloudNativeKit.Domain;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace CloudNativeKit.Infrastructure.Bus.Messaging
{
    public class Outbox : AggregateRootBase<Guid>
    {
        public DateTime OccurredOn { get; private set; }

        public string Type { get; private set; }

        public string Data { get; private set; }

        public DateTime? ProcessedDate { get; private set; }

        public Outbox(Guid id, DateTime occurredOn, string type, string data)
        {
            Id = id.Equals(Guid.Empty) ? NewId() : id;
            OccurredOn = occurredOn;
            Type = type;
            Data = data;
        }
    }
}
