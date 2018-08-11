using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice.ConfigureServices
{
  public class ApiVersionConfigureService : IBasicConfigureServices
  {
    public int Priority { get; } = 400;

    public void Configure(IServiceCollection services)
    {
      var svcProvider = services.BuildServiceProvider();
      var config = svcProvider.GetRequiredService<IConfiguration>();

      services.AddRouting(o => o.LowercaseUrls = true);
      services
        .AddMvcCore()
        .AddVersionedApiExplorer(
          o =>
          {
            o.GroupNameFormat = "'v'VVV";
            o.SubstituteApiVersionInUrl = true;
          })
        .AddJsonFormatters(o => o.ContractResolver = new CamelCasePropertyNamesContractResolver())
        .AddDataAnnotations();

      services.AddApiVersioning(o =>
      {
        o.ReportApiVersions = true;
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = ParseApiVersion(config.GetValue<string>("API_VERSION"));
      });
    }

    private static ApiVersion ParseApiVersion(string serviceVersion)
    {
      if (string.IsNullOrEmpty(serviceVersion))
      {
        throw new Exception("[CS] ServiceVersion is null or empty.");
      }

      const string pattern = @"(.)|(-)";
      var results = Regex.Split(serviceVersion, pattern)
        .Where(x => x != string.Empty && x != "." && x != "-")
        .ToArray();

      if (results == null || results.Count() < 2)
      {
        throw new Exception("[CS] Could not parse ServiceVersion.");
      }

      if (results.Count() > 2)
      {
        return new ApiVersion(
          Convert.ToInt32(results[0]),
          Convert.ToInt32(results[1]),
          results[2]);
      }

      return new ApiVersion(
        Convert.ToInt32(results[0]),
        Convert.ToInt32(results[1]));
    }
  }
}
