using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VND.CoolStore.ShoppingCart.ProcessingServices
{
    public interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    public class ScopedProcessingService : IScopedProcessingService
    {
        private readonly IEfUnitOfWork<MessagingDataContext> _unitOfWork;
        private readonly IMessageBus _messageBus;
        private readonly ILogger<ScopedProcessingService> _logger;

        public ScopedProcessingService(
            IEfUnitOfWork<MessagingDataContext> unitOfWork,
            IMessageBus messageBus,
            ILogger<ScopedProcessingService> logger)
        {
            _unitOfWork = unitOfWork;
            _messageBus = messageBus;
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var @events = _unitOfWork.QueryRepository<Outbox, Guid>()
                    .Queryable()
                    .Where(evt => evt.ProcessedDate == null)
                    .ToList();

                if (@events.Count() > 0)
                {
                    var commandRepo = _unitOfWork.RepositoryAsync<Outbox, Guid>();

                    foreach (var @event in @events)
                    {
                        var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                            .SingleOrDefault(assembly =>
                                assembly.GetName().Name.Contains("VND.CoolStore.ShoppingCart.DataContracts") &&
                                @event.Type.Contains(assembly.GetName().Name));

                        var type = messageAssembly.GetType(@event.Type);
                        var integrationEvent = (dynamic)JsonConvert.DeserializeObject(@event.Data, type);

                        try
                        {
                            await _messageBus.PublishAsync(integrationEvent, "shopping_cart_service");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }

                        @event.UpdateProcessedDate();
                        await commandRepo.UpdateAsync(@event);
                        await _unitOfWork.SaveChangesAsync(default);
                    }
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
