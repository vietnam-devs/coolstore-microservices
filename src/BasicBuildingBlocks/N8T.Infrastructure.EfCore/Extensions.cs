using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using N8T.Domain;

namespace N8T.Infrastructure.EfCore
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomDbContext<TDbContext, TType>(this IServiceCollection services, string connString)
            where TDbContext : DbContext, IDbFacadeResolver, IDomainEventContext
        {
            services.AddPooledDbContextFactory<TDbContext>(options =>
                {
                    options.UseNpgsql(connString, sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(TType).Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    }).UseSnakeCaseNamingConvention();
                });

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<IDbContextFactory<TDbContext>>()!.CreateDbContext());
            services.AddScoped<IDomainEventContext>(provider => provider.GetService<IDbContextFactory<TDbContext>>()!.CreateDbContext());

            services.AddHostedService<DbContextMigratorHostedService>();

            return services;
        }

        public static async ValueTask<TResponse> HandleTransaction<TDbContext, TResponse>(this IMediator mediator,
            TDbContext dbContext, CancellationToken cancellationToken, Func<Task<TResponse>> next)
            where TDbContext : DbContext, IDomainEventContext
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                // Achieving atomicity
                await using var transaction =
                    await dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                var response = await next();

                await transaction.CommitAsync(cancellationToken);

                var domainEvents = dbContext.GetDomainEvents().ToList();

                var tasks = domainEvents
                    .Select(async @event =>
                    {
                        //IMPORTANT: because we have int identity
                        var id = (response as dynamic)?.Id;
                        @event.MetaData.Add("id", id);

                        // publish it out
                        await mediator.Publish(@event, cancellationToken);
                    });

                await Task.WhenAll(tasks);

                return response;
            });
        }

        public static void MigrateDataFromScript(this MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetCallingAssembly();
            var files = assembly.GetManifestResourceNames();
            var filePrefix = $"{assembly.GetName().Name}.Infrastructure.Persistence.Scripts."; //IMPORTANT

            foreach (var file in files
                .Where(f => f.StartsWith(filePrefix) && f.EndsWith(".sql"))
                .Select(f => new {PhysicalFile = f, LogicalFile = f.Replace(filePrefix, string.Empty)})
                .OrderBy(f => f.LogicalFile))
            {
                using var stream = assembly.GetManifestResourceStream(file.PhysicalFile);
                using var reader = new StreamReader(stream!);
                var command = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(command))
                    continue;

                migrationBuilder.Sql(command);
            }
        }
    }
}
