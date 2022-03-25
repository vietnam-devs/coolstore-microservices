using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using MediatR;
using Microsoft.Extensions.Options;
using N8T.Core.Domain;

namespace N8T.Infrastructure.TxOutbox.Dapr.Internal
{
    internal class DaprLocalDispatchedHandler : INotificationHandler<EventWrapper>
    {
        private readonly DaprClient _daprClient;
        private readonly IOptions<DaprTxOutboxOptions> _options;

        public DaprLocalDispatchedHandler(DaprClient daprClient, IOptions<DaprTxOutboxOptions> options)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Handle(EventWrapper @eventWrapper, CancellationToken cancellationToken)
        {
            var events = await _daprClient.GetStateEntryAsync<List<OutboxEntity>>(_options.Value.StateStoreName, _options.Value.OutboxName, cancellationToken: cancellationToken);
            events.Value ??= new List<OutboxEntity>();

            var outboxEntity = new OutboxEntity(Guid.NewGuid(), System.DateTime.UtcNow, @eventWrapper.Event);

            events.Value.Add(outboxEntity);

            await events.SaveAsync(cancellationToken: cancellationToken);
        }
    }
}
