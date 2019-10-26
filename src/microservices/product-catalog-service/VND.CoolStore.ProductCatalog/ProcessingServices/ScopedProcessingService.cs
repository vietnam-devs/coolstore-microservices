using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using Microsoft.Extensions.Logging;

namespace VND.CoolStore.ProductCatalog.ProcessingServices
{
    public class ScopedProcessingService : OutboxScopedProcessingServiceBase, IScopedProcessingService
    {
        public ScopedProcessingService(IEfUnitOfWork<MessagingDataContext> unitOfWork, IMessageBus messageBus, ILogger<ScopedProcessingService> logger)
            : base(unitOfWork, messageBus, logger)
        {
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await PublishEventsUnProcessInOutboxToChannels("product_catalog_service");
                await Task.Delay(5000, stoppingToken);
            }
        }

        public override bool ScanAssemblyWithConditions(Assembly assembly)
        {
            return assembly.GetName().Name.Contains("VND.CoolStore.ProductCatalog.DataContracts");
        }
    }
}
