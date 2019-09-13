using System.Threading.Tasks;
using MediatR;
using CloudNativeKit.Domain;

namespace CloudNativeKit.Infrastructure.Bus.Internal
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Dispatch(IEvent @event)
        {
            await _mediator.Publish(new NotificationEnvelope(@event));
        }

        public void Dispose()
        {
        }
    }
}
