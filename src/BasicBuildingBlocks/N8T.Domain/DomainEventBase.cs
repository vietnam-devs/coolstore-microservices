using System;
using System.Collections.Generic;
using MediatR;

namespace N8T.Domain
{
    public interface IDomainEvent : INotification
    {
        DateTime CreatedAt { get; }
        IDictionary<string, object> MetaData { get; }
    }

    public interface IDomainEventContext
    {
        IEnumerable<DomainEventBase> GetDomainEvents();
    }

    public abstract class EventBase : IDomainEvent
    {
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public IDictionary<string, object> MetaData { get; } = new Dictionary<string, object>();
    }

    public abstract class DomainEventBase : EventBase
    {
    }

    public abstract class IntegrationEventBase : EventBase
    {
    }
}
