using Microsoft.Extensions.DependencyInjection;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db;
using CloudNativeKit.Infrastructure.DataPersistence.InMemory;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Command;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Query;
using Microsoft.Extensions.Configuration;
using CloudNativeKit.Infrastructure.DataPersistence.Dapper;

namespace CloudNativeKit.Infrastructure.DataPersistence
{
    public static class Startup
    {
        public static IServiceCollection AddEfGenericRepository(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Scoped<IUnitOfWorkAsync, UnitOfWork>());
            services.Add(ServiceDescriptor.Scoped<IQueryRepositoryFactory, QueryRepositoryFactory>());
            return services;
        }

        public static IServiceCollection AddInMemoryDb(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Scoped<IDbConnStringFactory, NoOpDbConnStringFactory>());
            services.Add(ServiceDescriptor.Scoped<IExtendDbContextOptionsBuilder, InMemoryDbContextOptionsBuilderFactory>());
            return services;
        }

        public static IServiceCollection AddDapper(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var svcProvider = scope.ServiceProvider;
            var config = svcProvider.GetRequiredService<IConfiguration>();

            services.Configure<DapperDbOptions>(config.GetSection("Dapper:DbServer"));
            services.Add(ServiceDescriptor.Scoped<IDbConnStringFactory, Dapper.Internal.SqlDbConnStringFactory>());
            services.Add(ServiceDescriptor.Transient<ISqlConnectionFactory, Dapper.Internal.SqlConnectionFactory>());
            services.Add(ServiceDescriptor.Transient<IDynamicSqlConnectionFactory, Dapper.Internal.DynamicSqlConnectionFactory>());
            return services;
        }

        public static IServiceCollection AddDynamicDapper(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Transient<IDynamicSqlConnectionFactory, Dapper.Internal.DynamicSqlConnectionFactory>());
            return services;
        }
    }
}
