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

						var k8s = _config.GetSection("k8sdb");
						var k8sHost = k8s.GetValue<string>("Host");
						var k8sPort = k8s.GetValue<string>("Port");
						var k8sDatabase = k8s.GetValue<string>("Database");
						var k8sUserName = k8s.GetValue<string>("UserName");
						var k8sPassword = k8s.GetValue<string>("Password");
						var connectionString = _config.GetConnectionString("k8s_mssql");

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
