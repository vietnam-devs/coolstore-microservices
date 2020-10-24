using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
            services
                .AddDbContext<TDbContext>(options =>
                {
                    options.UseSqlServer(connString, sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(TType).Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, TimeSpan.FromSeconds(10), null);
                    });
                });

            services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<TDbContext>());
            services.AddScoped<IDomainEventContext>(provider => provider.GetService<TDbContext>());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TxBehavior<,>));
            services.AddHostedService<DbContextMigratorHostedService>();

            return services;
        }

        public static void MigrateDataFromScript(this MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null) return;
            var files = assembly.GetManifestResourceNames();
            var filePrefix = $"{assembly.GetName().Name}.Core.Infrastructure.Persistence.Scripts.";

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
