using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VND.CoolStore.Services.Inventory.UseCases.Service;
using VND.CoolStore.Services.Inventory.UseCases.Service.Impl;
using VND.FW.Infrastructure.EfCore.Db;
using VND.FW.Infrastructure.EfCore.Extensions;
using VND.FW.Infrastructure.EfCore.Options;
using VND.FW.Infrastructure.EfCore.SqlServer;

namespace VND.CoolStore.Services.Inventory
{
		public class Startup
		{
				public Startup(IConfiguration configuration)
				{
						Configuration = configuration;
				}

				public IConfiguration Configuration { get; }

				public void ConfigureServices(IServiceCollection services)
				{
						services.AddEfCore();
						services.AddEfCoreSqlServer();

						services.AddScoped<IInventoryService, InventoryService>();
						services.AddRouting(options => options.LowercaseUrls = true);
						services.AddOptions()
								.Configure<EfCoreOption>(Configuration.GetSection("EfCore"));

						var serviceProvider = services.BuildServiceProvider();
						var extendOptionsBuilder = serviceProvider.GetService<IExtendDbContextOptionsBuilder>();
						var dbConnectionStringFactory = serviceProvider.GetService<IDatabaseConnectionStringFactory>();

						void optionsBuilderAction(DbContextOptionsBuilder optionsBuilder)
						{
								extendOptionsBuilder.Extend(
										optionsBuilder,
										dbConnectionStringFactory,
										typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
						}

						services.AddDbContext<ApplicationDbContext>(options => optionsBuilderAction(options));
						services.AddScoped<DbContext>(resolver => resolver.GetRequiredService<ApplicationDbContext>());

						services.AddMvcCore().AddVersionedApiExplorer(
							options =>
							{
									options.GroupNameFormat = "'v'VVV";
									options.SubstituteApiVersionInUrl = true;
							});

						services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

						services.AddApiVersioning(o =>
						{
								o.ReportApiVersions = true;
						});
				}

				public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
				{
						loggerFactory.AddConsole(Configuration.GetSection("Logging"));
						loggerFactory.AddDebug();

						if (env.IsDevelopment())
						{
								app.UseDeveloperExceptionPage();
						}

						app.UseMvc();
				}
		}
}
