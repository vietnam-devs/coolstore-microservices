using System;
using System.Collections.Generic;

namespace N8T.Domain
{
    public abstract class EntityBase
    {
        public DateTime Created { get; protected set; } = DateTime.UtcNow;
        public DateTime? Updated { get; protected set; }
        public HashSet<DomainEventBase> DomainEvents { get; private set; }

        public void AddDomainEvent(DomainEventBase eventItem)
        {
            DomainEvents ??= new HashSet<DomainEventBase>();
            DomainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(DomainEventBase eventItem)
        {
            DomainEvents?.Remove(eventItem);
        }
    }
}
