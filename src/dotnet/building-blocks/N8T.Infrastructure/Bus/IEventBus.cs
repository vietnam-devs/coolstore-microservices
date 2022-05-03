using System.Threading;
using System.Threading.Tasks;
using N8T.Core.Domain;

namespace N8T.Infrastructure.Bus
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event, string[] topics = default, CancellationToken token = default)
            where TEvent : IDomainEvent;

        Task SubscribeAsync<TEvent>(string[] topics = default, CancellationToken token = default)
            where TEvent : IDomainEvent;
    }
}
