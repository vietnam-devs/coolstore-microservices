using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;

namespace VND.CoolStore.ShoppingCart.Listeners
{
    public class MessageEnvelopeListener : INotificationHandler<MessageEnvelope<ProductUpdated>>
    {
        public Task Handle(MessageEnvelope<ProductUpdated> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
