using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VND.Fw.Infrastructure.EfCore.Db;
using VND.Fw.Infrastructure.EfCore.SqlServer.Options;

namespace VND.Fw.Infrastructure.EfCore.SqlServer
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddEfCoreSqlServer(this IServiceCollection services)
    {
      var svcProvider = services.BuildServiceProvider();
      var config = svcProvider.GetRequiredService<IConfiguration>();

      services.Configure<MsSqlDbOptions>(config.GetSection("k8s:mssqldb"));

      services.AddScoped<IExtendDbContextOptionsBuilder, SqlServerDbContextOptionsBuilderFactory>();
      services.AddScoped<IDatabaseConnectionStringFactory, SqlServerDatabaseConnectionStringFactory>();

      return services;
    }
  }
}
