using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using MediatR;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;
using VND.CoolStore.ShoppingCart.Usecases.SyncProductCatalogInfo;

namespace VND.CoolStore.ShoppingCart.Api.Listeners
{
    public class MessageEnvelopeListener : INotificationHandler<MessageEnvelope<ProductUpdated>>
    {
        private readonly ISyncProductCatalogInfoService _syncProductCatalogInfoService;

        public MessageEnvelopeListener(ISyncProductCatalogInfoService syncProductCatalogInfoService)
        {
            _syncProductCatalogInfoService = syncProductCatalogInfoService;
        }

        public async Task Handle(MessageEnvelope<ProductUpdated> notification, CancellationToken cancellationToken)
        {
            if(notification.Message is ProductUpdated)
            {
                await _syncProductCatalogInfoService.SyncData(notification.Message, cancellationToken);
            }
            else if (notification.Message is ProductDeleted)
            {
                //TODO: mark it as deleted state 
            }
        }
    }
}
