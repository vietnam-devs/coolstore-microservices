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
						var k8s = _config.GetSection("K8s");
						var serviceHost = k8s.GetValue<string>("DbServiceHost");
						var servicePort = k8s.GetValue<string>("DbServicePort");
						var connectionString = _config.GetConnectionString("SqlServerDefault");

						if (_env.IsDevelopment())
						{
								return string.Format(connectionString, "127.0.0.1", 1433);
						}

						return string.Format(
								connectionString, 
								Environment.GetEnvironmentVariable(serviceHost),
								Environment.GetEnvironmentVariable(servicePort));
				}
		}
}
