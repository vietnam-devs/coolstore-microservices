using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db;

namespace CloudNativeKit.Infrastructure.DataPersistence.EfCore
{
    public static class Startup
    {
        public static IServiceCollection AddGenericRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWorkAsync, UnitOfWork>();
            services.AddScoped<IQueryRepositoryFactory, QueryRepositoryFactory>();
            return services;
        }

        public static IServiceCollection AddInMemoryDb(this IServiceCollection services)
        {
            services.AddScoped<IDbConnStringFactory, NoOpDbConnStringFactory>();
            services.AddScoped<IExtendDbContextOptionsBuilder, InMemoryDbContextOptionsBuilderFactory>();
            return services;
        }
    }

    internal class NoOpDbConnStringFactory : IDbConnStringFactory
    {
        public string Create()
        {
            return string.Empty;
        }
    }

    internal class InMemoryDbContextOptionsBuilderFactory : IExtendDbContextOptionsBuilder
    {
        public DbContextOptionsBuilder Extend(
            DbContextOptionsBuilder optionsBuilder,
            IDbConnStringFactory connStringFactory,
            string assemblyName)
        {
            return optionsBuilder.UseInMemoryDatabase("defaultdb");
        }
    }
}
