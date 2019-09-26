using System;
using System.Threading;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Bus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.Inventory.Workers
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider services, ILogger<Worker> logger)
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
            using var scope = Services.CreateScope();
            var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
            await scopedProcessingService.DoWork(stoppingToken);
        }
    }

    public interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    public class ScopedProcessingService : IScopedProcessingService
    {
        private readonly IMessageSubscriber _messageSubscriber;
        private readonly ILogger<ScopedProcessingService> _logger;

        public ScopedProcessingService(
            IMessageSubscriber messageSubscriber,
            ILogger<ScopedProcessingService> logger)
        {
            _messageSubscriber = messageSubscriber;
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _messageSubscriber.SubscribeAsync<ShoppingCartWithProductCreated>("shopping_cart_service");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    public class MessageEnvelopeHandler : INotificationHandler<MessageEnvelope<ShoppingCartWithProductCreated>>
    {
        public Task Handle(MessageEnvelope<ShoppingCartWithProductCreated> notification, CancellationToken cancellationToken)
        {
            switch (notification.Message)
            {
                case ShoppingCartWithProductCreated message:

                    // do something
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
