using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VND.Fw.Infrastructure.AspNetCore.Extensions;
using VND.Fw.Infrastructure.AspNetCore.OpenApi;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice.ConfigureServices
{
  public class OpenApiConfigureService : IBasicConfigureServices
  {
    public int Priority { get; } = 800;

    public void Configure(IServiceCollection services)
    {
      var svcProvider = services.BuildServiceProvider();
      var config = svcProvider.GetRequiredService<IConfiguration>();
      var serviceParams = svcProvider.GetRequiredService<ServiceParams>();

      if (config.GetSection("OpenApi") == null)
      {
        throw new Exception("Please add OpenApi configuration or disabled OpenAPI.");
      }

      if (config.GetValue("OpenApi:Enabled", false))
      {
        services.Configure<OpenApiOptions>(config.GetSection("OpenApi"));

        services.AddSwaggerGen(c =>
        {
          var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

          c.DescribeAllEnumsAsStrings();

          foreach (var description in provider.ApiVersionDescriptions)
          {
            c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(config, description));
          }

          // c.IncludeXmlComments (XmlCommentsFilePath);

          if (config.GetValue("EnableAuthN", false))
          {
            c.AddSecurityDefinition("oauth2", new OAuth2Scheme
            {
              Type = "oauth2",
              Flow = "implicit",
              AuthorizationUrl = $"{GetExternalAuthUri(config)}/connect/authorize",
              TokenUrl = $"{GetExternalAuthUri(config)}/connect/token",
              Scopes = serviceParams["scopes"] as Dictionary<string, string>
            });
          }

          c.EnableAnnotations();

          if (config.GetValue("EnableAuthN", false))
          {
            c.OperationFilter<SecurityRequirementsOperationFilter>();
          }

          c.OperationFilter<DefaultValuesOperationFilter>();
          c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });
      }
    }

    private static string GetExternalAuthUri(IConfiguration config)
    {
      return config.GetExternalHostUri("Auth");
    }

    private static Info CreateInfoForApiVersion(IConfiguration config, ApiVersionDescription description)
    {
      var info = new Info()
      {
        Title = $"{config.GetValue("OpenApi:Title", "API")} {description.ApiVersion}",
        Version = description.ApiVersion.ToString(),
        Description = config.GetValue("OpenApi:Description", "An application with Swagger, Swashbuckle, and API versioning."),
        Contact = new Contact()
        {
          Name = config.GetValue("OpenApi:ContactName", "Vietnam Devs"),
          Email = config.GetValue("OpenApi:ContactEmail", "vietnam.devs.group@gmail.com")
        },
        TermsOfService = config.GetValue("OpenApi:TermOfService", "Shareware"),
        License = new License()
        {
          Name = config.GetValue("OpenApi:LicenseName", "MIT"),
          Url = config.GetValue("OpenApi:LicenseUrl", "https://opensource.org/licenses/MIT")
        }
      };

      if (description.IsDeprecated)
      {
        info.Description += " This API version has been deprecated.";
      }

      return info;
    }
  }
}
