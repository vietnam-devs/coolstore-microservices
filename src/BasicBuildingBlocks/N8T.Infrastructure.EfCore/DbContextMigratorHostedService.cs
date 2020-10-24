using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace N8T.Infrastructure.EfCore
{
    public class DbContextMigratorHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DbContextMigratorHostedService> _logger;

        public DbContextMigratorHostedService(IServiceProvider serviceProvider,
            ILogger<DbContextMigratorHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var policy = CreatePolicy(3, _logger, nameof(DbContextMigratorHostedService));
            await policy.ExecuteAsync(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var dbFacadeResolver = scope.ServiceProvider.GetRequiredService<IDbFacadeResolver>();
                await dbFacadeResolver.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Done migration database schema.");
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static AsyncRetryPolicy CreatePolicy(int retries, ILogger logger, string prefix)
        {
            return Policy.Handle<SqlException>().WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(exception,
                        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        prefix, exception.GetType().Name, exception.Message, retry, retries);
                }
            );
        }
    }
}
