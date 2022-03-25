using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using N8T.Core.Domain;
using N8T.Core.Repository;
using Polly;
using Polly.Retry;

namespace N8T.Infrastructure.EfCore
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgresDbContext<TDbContext>(this IServiceCollection services,
            string connString, Action<DbContextOptionsBuilder> doMoreDbContextOptionsConfigure = null,
            Action<IServiceCollection> doMoreActions = null)
                where TDbContext : DbContext, IDbFacadeResolver, IDomainEventContext
        {
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(connString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                }).UseSnakeCaseNamingConvention();

                doMoreDbContextOptionsConfigure?.Invoke(options);
            });

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<TDbContext>());
            services.AddScoped<IDomainEventContext>(provider => provider.GetService<TDbContext>());

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TxBehavior<,>));

            doMoreActions?.Invoke(services);

            return services;
        }

        public static IServiceCollection AddRepository(this IServiceCollection services, Type repoType)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(repoType)
                .AddClasses(classes =>
                    classes.AssignableTo(repoType)).As(typeof(IRepository<>)).WithScopedLifetime()
                .AddClasses(classes =>
                    classes.AssignableTo(repoType)).As(typeof(IGridRepository<>)).WithScopedLifetime()
            );

            return services;
        }

        public static void MigrateDataFromScript(this MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetCallingAssembly();
            var files = assembly.GetManifestResourceNames();
            var filePrefix = $"{assembly.GetName().Name}.Data.Scripts."; //IMPORTANT

            foreach (var file in files
                .Where(f => f.StartsWith(filePrefix) && f.EndsWith(".sql"))
                .Select(f => new { PhysicalFile = f, LogicalFile = f.Replace(filePrefix, string.Empty) })
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

        public static async Task DoDbMigrationAsync(this IApplicationBuilder app, ILogger logger)
        {
            var scope = app.ApplicationServices.CreateAsyncScope();
            var dbFacadeResolver = scope.ServiceProvider.GetService<IDbFacadeResolver>();

            var policy = CreatePolicy(3, logger, nameof(WebApplication));
            await policy.ExecuteAsync(async () =>
            {
                if (!await dbFacadeResolver?.Database.CanConnectAsync()!)
                {
                    Console.WriteLine($"Connection String: {dbFacadeResolver?.Database.GetConnectionString()}");
                    throw new Exception("Couldn't connect database.");
                }

                var migrations = await dbFacadeResolver?.Database.GetPendingMigrationsAsync()!;
                if (migrations.Any())
                {
                    await dbFacadeResolver?.Database.MigrateAsync()!;
                    logger?.LogInformation("Migration database schema. Done!!!");
                }
            });

            static AsyncRetryPolicy CreatePolicy(int retries, ILogger logger, string prefix)
            {
                return Policy.Handle<Exception>().WaitAndRetryAsync(
                    retries,
                    retry => TimeSpan.FromSeconds(15),
                    (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception,
                            "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}",
                            prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
            }
        }
    }
}
