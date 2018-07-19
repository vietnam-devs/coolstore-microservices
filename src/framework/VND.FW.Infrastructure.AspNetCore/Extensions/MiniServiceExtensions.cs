using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using VND.FW.Infrastructure.EfCore.Db;
using VND.FW.Infrastructure.EfCore.Extensions;
using VND.FW.Infrastructure.EfCore.Options;

namespace VND.FW.Infrastructure.AspNetCore.Extensions
{
  public static class MiniServiceExtensions
  {
    public static IServiceCollection AddMiniService(this IServiceCollection services, Assembly startupAssembly)
    {
      ServiceProvider serviceProvider = services.BuildServiceProvider();
      IConfiguration config = serviceProvider.GetService<IConfiguration>();
      IExtendDbContextOptionsBuilder extendOptionsBuilder = serviceProvider.GetService<IExtendDbContextOptionsBuilder>();
      IDatabaseConnectionStringFactory dbConnectionStringFactory = serviceProvider.GetService<IDatabaseConnectionStringFactory>();

      services.AddEfCore();

      services.AddRouting(options => options.LowercaseUrls = true);
      services.AddOptions()
          .Configure<PersistenceOption>(config.GetSection("EfCore"));

      void optionsBuilderAction(DbContextOptionsBuilder optionsBuilder)
      {
        extendOptionsBuilder.Extend(
            optionsBuilder,
            dbConnectionStringFactory,
            startupAssembly.GetName().Name);
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

      return services;
    }

    public static IApplicationBuilder UseMiniService(this IApplicationBuilder app)
    {
      IConfiguration config = app.ApplicationServices.GetService<IConfiguration>();
      IHostingEnvironment env = app.ApplicationServices.GetService<IHostingEnvironment>();
      ILoggerFactory loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

      loggerFactory.AddConsole(config.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();

      return app;
    }
  }
}
