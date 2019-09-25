using System;
using System.Threading;
using System.Threading.Tasks;
using static CloudNativeKit.Utils.Helpers.IdHelper;
using static CloudNativeKit.Utils.Helpers.DateTimeHelper;

namespace CloudNativeKit.Domain
{
    /// <summary>
    ///  Supertype for all Event types
    /// </summary>
    public interface IEvent
    {
        Guid Id { get; }
        int EventVersion { get; }
        DateTime OccurredOn { get; }
    }

    public interface IIntegrationEvent : IEvent
    {
    }

    public interface IEventHandler<in TEvent, TResult>
        where TEvent : IEvent
    {
        Task<TResult> Handle(TEvent request, CancellationToken cancellationToken);
    }

    public interface IDomainEventDispatcher : IDisposable
    {
        Task Dispatch(IEvent @event);
    }

    public abstract class EventBase : IEvent
    {
        public Guid Id { get; protected set; } = NewId();
        public int EventVersion { get; protected set; } = 1;
        public DateTime OccurredOn { get; protected set; } = NewDateTime();
    }

    public abstract class IntegrationEventBase : EventBase, IIntegrationEvent
    {
    }

    public class EventEnvelope : EventBase
    {
        public EventEnvelope(IEvent @event)
        {
            Event = @event;
        }

        public IEvent Event { get; }
    }

    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public void Dispose()
        {
        }

        public Task Dispatch(IEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
