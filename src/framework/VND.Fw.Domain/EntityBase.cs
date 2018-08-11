using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VND.Fw.Utils.Helpers;

namespace VND.Fw.Domain
{
  public interface IEntity : IIdentity
  { }

  public interface IAggregateRoot
  {
    IAggregateRoot ApplyEvent(IDomainEvent payload);
    List<IDomainEvent> GetUncommittedEvents();
    void ClearUncommittedEvents();
    IAggregateRoot RemoveEvent(IDomainEvent @event);
    IAggregateRoot AddEvent(IDomainEvent uncommittedEvent);
    IAggregateRoot RegisterHandler<T>(Action<T> handler);
  }

  public abstract class AggregateRootBase : EntityBase, IAggregateRoot
  {
    private readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();
    private readonly Dictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();

    protected AggregateRootBase() : this(IdHelper.GenerateId())
    {
    }

    protected AggregateRootBase(Guid id)
    {
      Id = id;
      Created = DateTimeHelper.GenerateDateTime();
    }

    public int Version { get; protected set; }

    public IAggregateRoot AddEvent(IDomainEvent uncommittedEvent)
    {
      _uncommittedEvents.Add(uncommittedEvent);
      ApplyEvent(uncommittedEvent);
      return this;
    }

    public IAggregateRoot ApplyEvent(IDomainEvent payload)
    {
      _handlers[payload.GetType()](payload);
      Version++;
      return this;
    }

    public void ClearUncommittedEvents()
    {
      _uncommittedEvents.Clear();
    }

    public List<IDomainEvent> GetUncommittedEvents()
    {
      return _uncommittedEvents;
    }

    public IAggregateRoot RegisterHandler<T>(Action<T> handler)
    {
      _handlers.Add(typeof(T), e => handler((T)e));
      return this;
    }

    public IAggregateRoot RemoveEvent(IDomainEvent @event)
    {
      if (_uncommittedEvents.Find(e => e == @event) != null)
      {
        _uncommittedEvents.Remove(@event);
      }
      return this;
    }
  }

  public abstract class EntityBase : IEntity
  {
    protected EntityBase() : this(IdHelper.GenerateId())
    {
    }

    protected EntityBase(Guid id)
    {
      Id = id;
      Created = DateTimeHelper.GenerateDateTime();
    }

    [Key]
    public Guid Id { get; protected set; }

    public DateTime Created { get; protected set; }

    public DateTime Updated { get; protected set; }
  }
}
