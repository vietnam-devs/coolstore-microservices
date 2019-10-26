using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using Microsoft.Extensions.Logging;
using VND.CoolStore.ProductCatalog.DataContracts.Event.V1;

namespace VND.CoolStore.Search.ProcessingServices
{
    public class ScopedProcessingService : ScopedProcessingServiceBase, IScopedProcessingService
    {
        public ScopedProcessingService(IMessageBus messageBus, ILogger<ScopedProcessingService> logger)
            : base(messageBus, logger)
        {
        }

        [DebuggerStepThrough]
        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await MessageBus.SubscribeAsync<ProductUpdated>("product_catalog_service");
                await MessageBus.SubscribeAsync<ProductDeleted>("product_catalog_service");

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
