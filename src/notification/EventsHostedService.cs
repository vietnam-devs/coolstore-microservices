using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreKit.Domain;

namespace VND.CoolStore.Services.Notification
{
  internal class EventsHostedService : IHostedService
  {
    private readonly ILogger _logger;
    private readonly IApplicationLifetime _appLifetime;
    private readonly IEventBus _eventBus;

    public EventsHostedService(
      ILogger<EventsHostedService> logger,
      IApplicationLifetime appLifetime,
      IEventBus eventBus)
    {
      _logger = logger;
      _appLifetime = appLifetime;
      _eventBus = eventBus;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _appLifetime.ApplicationStarted.Register(OnStarted);
      _appLifetime.ApplicationStopping.Register(OnStopping);
      _appLifetime.ApplicationStopped.Register(OnStopped);

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }

    private void OnStarted()
    {
      _logger.LogInformation("OnStarted has been called.");

      // _eventBus.Subscribe<ItemToCartAdded>().Wait();
    }

    private void OnStopping()
    {
      _logger.LogInformation("OnStopping has been called.");
    }

    private void OnStopped()
    {
      _logger.LogInformation("OnStopped has been called.");

      if(_eventBus != null)
        GC.SuppressFinalize(_eventBus);
    }
  }
}
