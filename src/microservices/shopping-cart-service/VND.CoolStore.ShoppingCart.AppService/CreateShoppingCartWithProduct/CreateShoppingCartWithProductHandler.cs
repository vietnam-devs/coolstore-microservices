using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using MediatR;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.CreateShoppingCartWithProduct
{
    public class CreateShoppingCartWithProductHandler : IRequestHandler<InsertItemToNewCartRequest, InsertItemToNewCartResponse>
    {
        public Task<InsertItemToNewCartResponse> Handle(InsertItemToNewCartRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new InsertItemToNewCartResponse());
        }
    }

    public class NotificationEnvelopeHandler : INotificationHandler<NotificationEnvelope>
    {
        public Task Handle(NotificationEnvelope notification, CancellationToken cancellationToken)
        {
            switch (notification.Event)
            {
                case ShoppingCartWithProductCreated @event:

                    // do something
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
