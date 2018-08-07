using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using VND.FW.Infrastructure.EfCore.Db;

namespace VND.FW.Infrastructure.EfCore.SqlServer
{
  public sealed class SqlServerDatabaseConnectionStringFactory : IDatabaseConnectionStringFactory
  {
    private readonly IConfiguration _config;
    private readonly IHostingEnvironment _env;

    public SqlServerDatabaseConnectionStringFactory(
      IConfiguration config,
      IHostingEnvironment env)
    {
      _config = config;
      _env = env;
    }

    public string Create()
    {
      if (_env.IsDevelopment())
      {
        return _config.GetConnectionString("mssqldb");
      }

      return string.Format(
          _config.GetConnectionString("mssqldb"),
          Environment.GetEnvironmentVariable(_config.GetValue<string>("k8s:mssqldb:Host")),
          Environment.GetEnvironmentVariable(_config.GetValue<string>("k8s:mssqldb:Port")),
          _config.GetValue<string>("k8s:mssqldb:Database"),
          _config.GetValue<string>("k8s:mssqldb:UserName"),
          _config.GetValue<string>("k8s:mssqldb:Password"));
    }
  }
}
