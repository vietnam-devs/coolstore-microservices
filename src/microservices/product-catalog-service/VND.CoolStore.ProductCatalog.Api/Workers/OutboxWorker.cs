using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VND.CoolStore.ProductCatalog.Api.Workers
{
    public class OutboxWorker : BackgroundService
    {
        private static object _locker = new object();
        private readonly ILogger<OutboxWorker> _logger;

        public OutboxWorker(IServiceProvider services, ILogger<OutboxWorker> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service running.");
            await DoWork(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");
            await Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is working.");

            var hasLock = false;

            try
            {
                Monitor.TryEnter(_locker, ref hasLock);

                if (!hasLock)
                {
                    return;
                }

                using var scope = Services.CreateScope();
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
                await scopedProcessingService.DoWork(stoppingToken);
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(_locker);
                }
            }
        }
    }
}
