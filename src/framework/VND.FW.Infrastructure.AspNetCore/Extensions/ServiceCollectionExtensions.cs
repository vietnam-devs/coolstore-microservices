using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Reflection;
using VND.FW.Infrastructure.EfCore.Db;
using VND.FW.Infrastructure.EfCore.Extensions;
using VND.FW.Infrastructure.EfCore.Options;

namespace VND.FW.Infrastructure.AspNetCore.Extensions
{
  public static class ServiceCollectionExtensions
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

    public static IServiceCollection AddHttpPolly(this IServiceCollection services)
    {
      services.AddHttpClient<RestClient>()
          .SetHandlerLifetime(TimeSpan.FromMinutes(1))
          .AddPolicyHandler(GetRetryPolicy())
          .AddPolicyHandler(GetCircuitBreakerPolicy());

      return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
      return HttpPolicyExtensions
          .HandleTransientHttpError()
          .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
          .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
      return HttpPolicyExtensions
          .HandleTransientHttpError()
          .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
  }
}
