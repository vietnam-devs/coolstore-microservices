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

    public SqlServerDatabaseConnectionStringFactory(IConfiguration config, IHostingEnvironment env)
    {
      _config = config;
      _env = env;
    }

    public string Create()
    {
      if (_env.IsDevelopment())
      {
        return _config.GetConnectionString("local_mssql");
      }

      IConfigurationSection k8s = _config.GetSection("k8sdb");
      string k8sHost = k8s.GetValue<string>("Host");
      string k8sPort = k8s.GetValue<string>("Port");
      string k8sDatabase = k8s.GetValue<string>("Database");
      string k8sUserName = k8s.GetValue<string>("UserName");
      string k8sPassword = k8s.GetValue<string>("Password");
      string connectionString = _config.GetConnectionString("k8s_mssql");

      return string.Format(
          connectionString,
          Environment.GetEnvironmentVariable(k8sHost),
          Environment.GetEnvironmentVariable(k8sPort),
          k8sDatabase,
          k8sUserName,
          k8sPassword);
    }
  }
}
