using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;

namespace VND.CoolStore.Search.Api.Listeners
{
    public class MessageEnvelopeListener :
        INotificationHandler<MessageEnvelope<ProductUpdated>>,
        INotificationHandler<MessageEnvelope<ProductDeleted>>
    {
        private readonly IMediator _mediator;

        public MessageEnvelopeListener(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(MessageEnvelope<ProductUpdated> notification, CancellationToken cancellationToken)
        {
            if (notification.Message is ProductUpdated productUpdated)
            {
                
            }
        }

        public async Task Handle(MessageEnvelope<ProductDeleted> notification, CancellationToken cancellationToken)
        {
            if (notification.Message is ProductDeleted productDeleted)
            {
            }
        }
    }
}
