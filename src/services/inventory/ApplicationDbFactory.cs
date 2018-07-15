using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using VND.FW.Infrastructure.EfCore.Db;
using VND.FW.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Inventory
{
		public class ApplicationDbFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
		{
				private readonly IExtendDbContextOptionsBuilder _extendOptionsBuilder;
				private readonly IDatabaseConnectionStringFactory _dbConnectionStringFactory;
				private readonly IConfiguration _config;

				public ApplicationDbFactory()
				{
						_config = new ConfigurationBuilder()
								.SetBasePath(Directory.GetCurrentDirectory())
								.AddJsonFile("appsettings.json", true, true)
								.Build();

						_extendOptionsBuilder = new SqlServerDbContextOptionsBuilderFactory();
						_dbConnectionStringFactory = new SqlServerDatabaseConnectionStringFactory(_config);
				}

				public ApplicationDbContext CreateDbContext(string[] args)
				{
						Console.WriteLine($"[VND] CONNECTION STRING: {_dbConnectionStringFactory.Create()}");

						var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
						Console.WriteLine($"[VND] ASSEMBLY FULL NAME: {migrationAssembly}");

						var dbContextOptionBuilder = _extendOptionsBuilder.Extend(
								new DbContextOptionsBuilder<ApplicationDbContext>(),
								_dbConnectionStringFactory,
								migrationAssembly);

						// Console.WriteLine($"[VND] ASSEMBLY NAME: {migrationAssembly.GetName().AssemblyQualifiedName}");

						return (ApplicationDbContext)Activator.CreateInstance(
								typeof(ApplicationDbContext),
								dbContextOptionBuilder.Options,
								_config
								);
				}
		}
}
