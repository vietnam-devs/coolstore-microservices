using System;
using System.Threading;
using System.Threading.Tasks;
using Avro.Specific;
using Confluent.SchemaRegistry;
using N8T.Core.Domain;
using N8T.Core.Repository;
using N8T.Infrastructure.SchemaRegistry;

namespace N8T.Infrastructure.EfCore;

public abstract class MutateHandlerBase<TOutBoxEntity>
    where TOutBoxEntity: Outbox, IAggregateRoot
{
    protected IRepository<TOutBoxEntity> OutboxRepository { get; }
    protected ISchemaRegistryClient SchemaRegistryClient { get; }

    protected MutateHandlerBase(ISchemaRegistryClient  schemaRegistryClient, IRepository<TOutBoxEntity> outboxRepository)
    {
        OutboxRepository = outboxRepository;
        SchemaRegistryClient = schemaRegistryClient;
    }

    protected async ValueTask ExportToOutbox<TRootEntity, TEvent>(
        TRootEntity rootEntity,
        Func<(TEvent, TOutBoxEntity, string)> eventFunc,
        CancellationToken cancellationToken)
        where TRootEntity : IAggregateRoot
        where TEvent: ISpecificRecord
    {
        var (@event, outboxEntity, topicName) = eventFunc();
        //var subject = SubjectNameStrategy.Topic.ConstructValueSubjectName(topicName);
        var eventBytes = await @event.SerializeAsync(SchemaRegistryClient, topicName);

        outboxEntity.Id = Guid.NewGuid();
        outboxEntity.Type = typeof(TEvent).Name;
        outboxEntity.AggregateType = typeof(TRootEntity).Name;
        outboxEntity.AggregateId = rootEntity.Id;
        outboxEntity.Payload = eventBytes;
        outboxEntity.Validate();

        // export it with Outbox pattern
        var outbox = await OutboxRepository.AddAsync(outboxEntity, cancellationToken: cancellationToken);
        await OutboxRepository.DeleteAsync(outbox, cancellationToken: cancellationToken); // avoid dirty in outbox table
    }
}
