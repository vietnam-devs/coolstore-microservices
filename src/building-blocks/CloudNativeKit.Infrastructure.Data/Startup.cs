using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudNativeKit.Infrastructure.Data
{
    public static class Startup
    {
        public static IServiceCollection AddEfInMemoryDb<TDbContext>(this IServiceCollection services, string dbContextAssemblyName) where TDbContext : DbContext
        {
            services.Add(ServiceDescriptor.Scoped<IDbConnStringFactory, InMemory.NoOpDbConnStringFactory>());
            services.Add(ServiceDescriptor.Scoped<EfCore.Core.Db.IExtendDbContextOptionsBuilder, InMemory.InMemoryDbContextOptionsBuilderFactory>());

            services.AddDbContext<TDbContext>((sp, o) =>
            {
                using var scope = sp.CreateScope();
                var resolver = scope.ServiceProvider;
                var config = resolver.GetService<IConfiguration>();
                var extendOptionsBuilder = resolver.GetRequiredService<EfCore.Core.Db.IExtendDbContextOptionsBuilder>();
                var connStringFactory = resolver.GetRequiredService<IDbConnStringFactory>();

                extendOptionsBuilder.Extend(o, connStringFactory, dbContextAssemblyName);
            });

            services.AddScoped<DbContext>(resolver => resolver.GetService<TDbContext>());
            services.Add(ServiceDescriptor.Scoped<EfCore.Core.IEfUnitOfWork, EfCore.Core.EfUnitOfWork>());

            return services;
        }

        public static IServiceCollection AddEfSqlServerDb<TDbContext>(this IServiceCollection services, string dbContextAssemblyName) where TDbContext : DbContext
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var resolver = scope.ServiceProvider;
            var config = resolver.GetService<IConfiguration>();
            services.Configure<EfCore.SqlServer.SqlServerDbOptions>(config.GetSection("ConnectionStrings"));

            services.Add(ServiceDescriptor.Scoped<IDbConnStringFactory, EfCore.SqlServer.SqlServerDbConnStringFactory>());
            services.Add(ServiceDescriptor.Scoped<EfCore.Core.Db.IExtendDbContextOptionsBuilder, EfCore.SqlServer.SqlServerDbContextOptionsBuilderFactory>());

            services.AddDbContext<TDbContext>((sp, o) =>
            {
                using var scope = sp.CreateScope();
                var resolver = scope.ServiceProvider;
                var config = resolver.GetService<IConfiguration>();
                var extendOptionsBuilder = resolver.GetRequiredService<EfCore.Core.Db.IExtendDbContextOptionsBuilder>();
                var connStringFactory = resolver.GetRequiredService<IDbConnStringFactory>();

                extendOptionsBuilder.Extend(o, connStringFactory, dbContextAssemblyName);
            });

            services.AddScoped<DbContext>(resolver => resolver.GetService<TDbContext>());
            services.Add(ServiceDescriptor.Scoped<EfCore.Core.IEfUnitOfWork, EfCore.Core.EfUnitOfWork>());

            return services;
        }

        public static IServiceCollection AddDapperComponents(this IServiceCollection services, bool dynamicSqlConnectionFactory = false)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var svcProvider = scope.ServiceProvider;
            var config = svcProvider.GetRequiredService<IConfiguration>();

            services.Configure<Dapper.DapperDbOptions>(config.GetSection("ConnectionStrings"));
            services.Add(ServiceDescriptor.Scoped<IDbConnStringFactory, Dapper.Core.SqlDbConnStringFactory>());

            if (!dynamicSqlConnectionFactory)
            {
                services.Add(ServiceDescriptor.Scoped<Dapper.ISqlConnectionFactory, Dapper.Core.SqlConnectionFactory>());
            }
            else
            {
                services.Add(ServiceDescriptor.Scoped<Dapper.IDynamicSqlConnectionFactory, Dapper.Core.DynamicSqlConnectionFactory>());
            }

            services.Add(ServiceDescriptor.Scoped<Dapper.Core.IDapperUnitOfWork, Dapper.Core.DapperUnitOfWork>());

            return services;
        }
    }
}
