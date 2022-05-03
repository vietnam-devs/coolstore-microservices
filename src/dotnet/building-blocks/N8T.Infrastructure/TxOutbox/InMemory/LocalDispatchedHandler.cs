using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using N8T.Core.Domain;

namespace N8T.Infrastructure.TxOutbox.InMemory
{
    public class LocalDispatchedHandler : INotificationHandler<EventWrapper>
    {
        private readonly IEventStorage _eventStorage;
        private readonly ILogger<LocalDispatchedHandler> _logger;

        public LocalDispatchedHandler(IEventStorage eventStorage, ILogger<LocalDispatchedHandler> logger)
        {
            _eventStorage = eventStorage ?? throw new ArgumentNullException(nameof(eventStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(EventWrapper @eventWrapper, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Store @event: {nameof(@eventWrapper.Event)} into the in-memory EventStore.");

            var outboxEntity = new OutboxEntity(Guid.NewGuid(), System.DateTime.UtcNow, @eventWrapper.Event);
            _eventStorage.Events.Add(outboxEntity);
            return Task.CompletedTask;
        }
    }
}
