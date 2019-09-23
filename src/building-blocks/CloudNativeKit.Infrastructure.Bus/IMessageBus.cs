using System.Threading.Tasks;
using CloudNativeKit.Domain;

namespace CloudNativeKit.Infrastructure.Bus
{
    public interface IMessageBus
    {
        Task PublishAsync<TMessage>(TMessage msg, params string[] channels) where TMessage : IIntegrationEvent;
        Task SubscribeAsync<TMessage>(params string[] channels) where TMessage : IIntegrationEvent, new();
    }
}
