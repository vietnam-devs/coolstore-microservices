using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.Bus;

namespace N8T.Infrastructure.TxOutbox.InMemory
{
    public class TxOutboxProcessor : ITxOutboxProcessor
    {
        private readonly IEventBus _eventBus;
        private readonly IEventStorage _eventStorage;
        private readonly ILogger<TxOutboxProcessor> _logger;

        public TxOutboxProcessor(IEventBus eventBus, IEventStorage eventStorage, ILogger<TxOutboxProcessor> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventStorage = eventStorage ?? throw new ArgumentNullException(nameof(eventStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(Type integrationAssemblyType, CancellationToken cancellationToken = new())
        {
            if (_eventStorage.Events.TryTake(out var domainEvent))
            {
                _logger.LogInformation($"Publish event: {nameof(domainEvent)} to the MessageBroker");
                var @event = domainEvent.RecreateMessage(integrationAssemblyType.Assembly);
                await _eventBus.PublishAsync(@event, token: cancellationToken);
            }
        }
    }
}
