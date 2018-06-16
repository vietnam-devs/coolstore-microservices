using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VND.Fw.Domain;
using VND.FW.Infrastructure.EfCore.Impl;

namespace VND.FW.Infrastructure.EfCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataModule(this IServiceCollection services)
        {
            services.AddScoped(typeof(IEfRepositoryAsync<>), typeof(EfRepositoryAsync<>));
            services.AddScoped(typeof(IEfQueryRepository<>), typeof(EfQueryRepository<>));

            services.AddScoped(
                typeof(IUnitOfWorkAsync), resolver =>
                new EfUnitOfWork(
                    resolver.GetService<DbContext>(),
                    resolver.GetService<IServiceProvider>()));

            // by default, we register the in-memory database
            services.AddScoped(typeof(IDatabaseConnectionStringFactory), typeof(NoOpDatabaseConnectionStringFactory));
            services.AddScoped(typeof(IExtendDbContextOptionsBuilder), typeof(InMemoryDbContextOptionsBuilderFactory));

            return services;
        }
    }

    public class NoOpDatabaseConnectionStringFactory : IDatabaseConnectionStringFactory
    {
        public string Create()
        {
            return string.Empty;
        }
    }

    public class InMemoryDbContextOptionsBuilderFactory : IExtendDbContextOptionsBuilder
    {
        public DbContextOptionsBuilder Extend(
            DbContextOptionsBuilder optionsBuilder, 
            IDatabaseConnectionStringFactory connectionStringFactory, 
            string assemblyName)
        {
            return optionsBuilder.UseSqlite(
                "Data Source=App_Data\\localdb.db",
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(assemblyName);
                });
        }
    }
}
