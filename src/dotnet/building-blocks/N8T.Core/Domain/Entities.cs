using System;
using System.Collections.Generic;
using System.Linq;

namespace N8T.Core.Domain
{
    public interface IEntity
    {
        public Guid Id { get; }
        public DateTime Created { get; }
        public DateTime? Updated { get; }
    }

    public interface IAggregateRoot : IEntity
    {
        public HashSet<EventBase> DomainEvents { get; }
    }

    public interface ITxRequest { }

    public abstract class EntityRootBase : EntityBase, IAggregateRoot
    {
        public HashSet<EventBase> DomainEvents { get; private set; }

        public void AddDomainEvent(EventBase eventItem)
        {
            DomainEvents ??= new HashSet<EventBase>();
            DomainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(EventBase eventItem)
        {
            DomainEvents?.Remove(eventItem);
        }
    }

    public abstract class EntityBase : IEntity
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Created { get; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        public DateTime? Updated { get; protected set; }
    }

    // Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }

            return left?.Equals(right) != false;
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left, right);
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
