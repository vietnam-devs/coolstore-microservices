using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using static CloudNativeKit.Utils.Helpers.IdHelper;
using static CloudNativeKit.Utils.Helpers.DateTimeHelper;

namespace CloudNativeKit.Domain
{
    public interface IAggregateRoot : IAggregateRootWithId<Guid>
    {
    }

    public interface IAggregateRootWithId<TId> : IEntityWithId<TId>
    {
        IAggregateRootWithId<TId> ApplyEvent(IEvent payload);
        List<IEvent> GetUncommittedEvents();
        void ClearUncommittedEvents();
        IAggregateRootWithId<TId> RemoveEvent(IEvent @event);
        IAggregateRootWithId<TId> AddEvent(IEvent uncommittedEvent);
        IAggregateRootWithId<TId> RegisterHandler<T>(Action<T> handler);
    }

    public abstract class AggregateRootBase : AggregateRootWithIdBase<Guid>, IAggregateRoot
    {
        protected AggregateRootBase() : base(GenerateId())
        {
        }
    }

    public abstract class AggregateRootWithIdBase<TId> : EntityWithIdBase<TId>, IAggregateRootWithId<TId>
    {
        private readonly IDictionary<Type, Action<object>> _handlers = new ConcurrentDictionary<Type, Action<object>>();
        private readonly List<IEvent> _uncommittedEvents = new List<IEvent>();

        protected AggregateRootWithIdBase() : this(default)
        {
        }

        protected AggregateRootWithIdBase(TId id) : base(id)
        {
            Created = GenerateDateTime();
        }

        public int Version { get; protected set; }

        public IAggregateRootWithId<TId> AddEvent(IEvent uncommittedEvent)
        {
            _uncommittedEvents.Add(uncommittedEvent);
            ApplyEvent(uncommittedEvent);
            return this;
        }

        public IAggregateRootWithId<TId> ApplyEvent(IEvent payload)
        {
            if (!_handlers.ContainsKey(payload.GetType()))
                return this;
            _handlers[payload.GetType()]?.Invoke(payload);
            Version++;
            return this;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public List<IEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public IAggregateRootWithId<TId> RegisterHandler<T>(Action<T> handler)
        {
            _handlers.Add(typeof(T), e => handler((T)e));
            return this;
        }

        public IAggregateRootWithId<TId> RemoveEvent(IEvent @event)
        {
            if (_uncommittedEvents.Find(e => e == @event) != null)
                _uncommittedEvents.Remove(@event);
            return this;
        }
    }
}
