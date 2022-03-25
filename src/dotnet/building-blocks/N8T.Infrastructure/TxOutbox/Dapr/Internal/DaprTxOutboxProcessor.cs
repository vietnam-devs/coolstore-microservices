using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N8T.Infrastructure.Bus;

namespace N8T.Infrastructure.TxOutbox.Dapr.Internal
{
    internal class DaprTxOutboxProcessor : ITxOutboxProcessor
    {
        private readonly DaprClient _daprClient;
        private readonly IEventBus _eventBus;
        private readonly IOptions<DaprTxOutboxOptions> _options;
        private readonly ILogger<DaprTxOutboxProcessor> _logger;

        public DaprTxOutboxProcessor(DaprClient daprClient, IEventBus eventBus, IOptions<DaprTxOutboxOptions> options, ILogger<DaprTxOutboxProcessor> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(Type integrationAssemblyType, CancellationToken cancellationToken = new())
        {
            _logger.LogTrace("{TransactionalOutboxProcessor}: Cron @{DateTime}", nameof(DaprTxOutboxProcessor), System.DateTime.UtcNow);

            var events = await _daprClient.GetStateEntryAsync<List<OutboxEntity>>(_options.Value.StateStoreName, _options.Value.OutboxName, cancellationToken: cancellationToken);

            if (events?.Value is not { Count: > 0 })
                return;

            var deletedEventIds = new List<Guid>();

            foreach (var domainEvent in events.Value)
            {
                if (domainEvent.Id.Equals(Guid.Empty) || string.IsNullOrEmpty(domainEvent.Type) || string.IsNullOrEmpty(domainEvent.Data))
                    continue;

                var @event = domainEvent.RecreateMessage(integrationAssemblyType.Assembly);

                await _eventBus.PublishAsync(@event, token: cancellationToken);

                deletedEventIds.Add(domainEvent.Id);
            }

            if (deletedEventIds.Count <= 0)
                return;

            foreach (var deletedEventId in deletedEventIds)
            {
                events.Value.RemoveAll(e => e.Id == deletedEventId);
            }

            await events.SaveAsync(cancellationToken: cancellationToken);
        }
    }
}
