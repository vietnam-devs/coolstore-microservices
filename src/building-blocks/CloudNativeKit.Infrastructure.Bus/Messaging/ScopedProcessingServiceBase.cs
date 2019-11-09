using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudNativeKit.Infrastructure.Bus.Messaging
{
    public abstract class ScopedProcessingServiceBase
    {
        protected ScopedProcessingServiceBase(IMessageBus messageBus, ILogger<ScopedProcessingServiceBase> logger)
        {
            MessageBus = messageBus;
            Logger = logger;
        }

        protected IMessageBus MessageBus { get; }
        protected ILogger<ScopedProcessingServiceBase> Logger { get; }
    }

    public abstract class OutboxScopedProcessingServiceBase : ScopedProcessingServiceBase
    {
        protected OutboxScopedProcessingServiceBase(IEfUnitOfWork<MessagingDataContext> unitOfWork, IMessageBus messageBus, ILogger<OutboxScopedProcessingServiceBase> logger)
            : base(messageBus, logger)
        {
            UnitOfWork = unitOfWork;
        }

        protected IEfUnitOfWork<MessagingDataContext> UnitOfWork { get; }


        public abstract bool ScanAssemblyWithConditions(Assembly assembly);

        [DebuggerStepThrough]
        protected async Task PublishEventsUnProcessInOutboxToChannels(params string[] channels)
        {
            if (UnitOfWork is null)
            {
                throw new Exception("UnitOfWork for MessageContext cannot be null.");
            }

            var @events = UnitOfWork.QueryRepository<Outbox, Guid>()
                    .Queryable()
                    .Where(evt => evt.ProcessedDate == null)
                    .ToList();

            Logger.LogDebug($"All events from Outbox are {JsonConvert.SerializeObject(@events)}.");

            if (@events.Count() > 0)
            {
                var commandRepo = UnitOfWork.RepositoryAsync<Outbox, Guid>();
                foreach (var @event in @events)
                {
                    var messageAssembly = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .SingleOrDefault(assembly =>
                            ScanAssemblyWithConditions(assembly) && @event.Type.Contains(assembly.GetName().Name));

                    Logger.LogDebug($"Found an assembly contains the message contract is {messageAssembly.GetName().Name}.");

                    var type = messageAssembly.GetType(@event.Type);
                    var integrationEvent = (dynamic)JsonConvert.DeserializeObject(@event.Data, type);

                    Logger.LogDebug($"Integration Event with content is {JsonConvert.SerializeObject(integrationEvent)}.");

                    try
                    {
                        await MessageBus.PublishAsync(integrationEvent, channels.ToArray());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    @event.UpdateProcessedDate();
                    await commandRepo.UpdateAsync(@event);
                    var rowCount = await UnitOfWork.SaveChangesAsync(default);
                    Logger.LogDebug($"{rowCount} rows saved into database.");
                }
            }
        }
    }
}
