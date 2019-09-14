using CloudNativeKit.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudNativeKit.Infrastructure.DataPersistence
{
    public static class Startup
    {
        public static IServiceCollection AddEfGenericRepository(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Scoped<IUnitOfWorkAsync, EfCore.Command.UnitOfWork>());
            services.Add(ServiceDescriptor.Scoped<IQueryRepositoryFactory, EfCore.Query.QueryRepositoryFactory>());
            return services;
        }

        public static IServiceCollection AddInMemoryDb(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Scoped<IDbConnStringFactory, InMemory.NoOpDbConnStringFactory>());
            services.Add(ServiceDescriptor.Scoped<EfCore.Db.IExtendDbContextOptionsBuilder, InMemory.InMemoryDbContextOptionsBuilderFactory>());
            return services;
        }

        public static IServiceCollection AddDapperGenericRepository(this IServiceCollection services)
        {
            //services.Add(ServiceDescriptor.Scoped<IUnitOfWorkAsync, EfCore.Command.UnitOfWork>());
            services.Add(ServiceDescriptor.Scoped<IQueryRepositoryFactory, Dapper.Query.QueryRepositoryFactory>());
            return services;
        }

        public static IServiceCollection AddDapper(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var svcProvider = scope.ServiceProvider;
            var config = svcProvider.GetRequiredService<IConfiguration>();

            services.Configure<Dapper.DapperDbOptions>(config.GetSection("Dapper:DbServer"));
            services.Add(ServiceDescriptor.Scoped<IDbConnStringFactory, Dapper.Db.SqlDbConnStringFactory>());
            services.Add(ServiceDescriptor.Transient<Dapper.ISqlConnectionFactory, Dapper.Db.SqlConnectionFactory>());
            services.Add(ServiceDescriptor.Transient<Dapper.IDynamicSqlConnectionFactory, Dapper.Db.DynamicSqlConnectionFactory>());
            return services;
        }

        public static IServiceCollection AddDynamicDapper(this IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Transient<Dapper.IDynamicSqlConnectionFactory, Dapper.Db.DynamicSqlConnectionFactory>());
            return services;
        }
    }
}
