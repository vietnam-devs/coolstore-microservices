using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.AspNetCore.Swagger;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice.ConfigureServices
{
  public class OpenApiConfigureService : IBasicConfigureServices
  {
    public int Priority { get; } = 800;

    public void Configure(IServiceCollection services)
    {
      var svcProvider = services.BuildServiceProvider();
      var config = svcProvider.GetRequiredService<IConfiguration>();
      var serviceParams = svcProvider.GetRequiredService<ServiceParams>();

      if (config.GetValue("EnableOpenApi", false))
      {
        services.AddSwaggerGen(c =>
        {
          var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

          c.DescribeAllEnumsAsStrings();

          foreach (var description in provider.ApiVersionDescriptions)
          {
            c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
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

          c.OperationFilter<SwaggerDefaultValuesOperationFilter>();
          c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });
      }
    }

    private static string GetExternalAuthUri(IConfiguration config)
    {
      return config.GetExternalHostUri("Auth");
    }

    /// <summary>
    /// TODO: refactoring to Options
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    private static Info CreateInfoForApiVersion(ApiVersionDescription description)
    {
      var info = new Info()
      {
        Title = $"API {description.ApiVersion}",
        Version = description.ApiVersion.ToString(),
        Description = "An application with Swagger, Swashbuckle, and API versioning.",
        Contact = new Contact()
        {
          Name = "VND",
          Email = "vietnam.devs.group@gmail.com"
        },
        TermsOfService = "Shareware",
        License = new License()
        {
          Name = "MIT",
          Url = "https://opensource.org/licenses/MIT"
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
