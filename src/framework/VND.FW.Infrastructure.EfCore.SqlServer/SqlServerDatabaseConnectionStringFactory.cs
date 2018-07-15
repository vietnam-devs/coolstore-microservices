using Microsoft.Extensions.Configuration;
using VND.FW.Infrastructure.EfCore.Db;

namespace VND.FW.Infrastructure.EfCore.SqlServer
{
		public sealed class SqlServerDatabaseConnectionStringFactory : IDatabaseConnectionStringFactory
		{
				private readonly IConfiguration _config;

				public SqlServerDatabaseConnectionStringFactory(IConfiguration config)
				{
						_config = config;
				}

				public string Create()
				{
						return _config.GetConnectionString("SqlServerDefault");
				}
		}
}
