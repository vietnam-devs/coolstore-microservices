using System.Collections.Concurrent;

namespace N8T.Infrastructure.TxOutbox.InMemory
{
    public interface IEventStorage
    {
        public ConcurrentBag<OutboxEntity> Events { get; }
    }

    public class EventStorage : IEventStorage
    {
        public ConcurrentBag<OutboxEntity> Events { get; } = new();
    }
}
