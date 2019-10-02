using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Utils.Extensions;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;
using VND.CoolStore.ShoppingCart.Usecases.MarkProductCatalogAsDeleted;
using VND.CoolStore.ShoppingCart.Usecases.ReplicateProductCatalogInfo;

namespace VND.CoolStore.ShoppingCart.Api.Listeners
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
                await _mediator.Send(new ReplicateProductCatalogInfo
                {
                    Id = productUpdated.ProductId.ConvertTo<Guid>(),
                    Name = productUpdated.Name,
                    Price = productUpdated.Price,
                    ImagePath = productUpdated.ImageUrl,
                    Description = productUpdated.Desc
                }, cancellationToken);
            }
        }

        public async Task Handle(MessageEnvelope<ProductDeleted> notification, CancellationToken cancellationToken)
        {
            if (notification.Message is ProductDeleted productDeleted)
            {
                await _mediator.Send(new MarkProductCatalogAsDeleted { ProductId = productDeleted.ProductId.ConvertTo<Guid>() }, cancellationToken);
            }
        }
    }
}
