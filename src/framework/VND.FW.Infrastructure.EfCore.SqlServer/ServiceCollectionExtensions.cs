using Microsoft.Extensions.DependencyInjection;
using VND.FW.Infrastructure.EfCore.Db;

namespace VND.FW.Infrastructure.EfCore.SqlServer
{
		public static class ServiceCollectionExtensions
		{
				public static IServiceCollection AddEfCoreSqlServer(this IServiceCollection services)
				{
						services.AddScoped<IExtendDbContextOptionsBuilder, SqlServerDbContextOptionsBuilderFactory>();
						services.AddScoped<IDatabaseConnectionStringFactory, SqlServerDatabaseConnectionStringFactory>();

						return services;
				}
		}
}
