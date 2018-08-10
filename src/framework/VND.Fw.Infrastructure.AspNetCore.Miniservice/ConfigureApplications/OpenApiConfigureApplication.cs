using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VND.Fw.Infrastructure.AspNetCore.Extensions;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice.ConfigureApplications
{
  public class OpenApiConfigureApplication : IConfigureApplication
  {
    public int Priority { get; } = 900;
    public void Configure(IApplicationBuilder app)
    {
      var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
      var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

      var basePath = config.GetBasePath();
      var currentHostUri = config.GetExternalCurrentHostUri();

      if (config.GetValue("EnableOpenApi", false))
      {
        app.UseSwagger();
      }

      if (config.GetValue("EnableOpenApiUi", false))
      {
        app.UseSwaggerUI(
          c =>
          {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            // build a swagger endpoint for each discovered API version
            foreach (var description in provider.ApiVersionDescriptions)
            {
              c.SwaggerEndpoint(
                $"{basePath}swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
            }

            if (config.GetValue("EnableAuthN", false))
            {
              c.OAuthClientId("swagger_id");
              c.OAuthClientSecret("secret".Sha256());
              c.OAuthAppName("swagger_app");
              c.OAuth2RedirectUrl($"{currentHostUri}/swagger/oauth2-redirect.html");
            }

            // c.IndexStream = () => typeof(MiniServiceExtensions).GetTypeInfo().Assembly.GetManifestResourceStream("VND.FW.Infrastructure.AspNetCore.Swagger.index.html");
          });
      }
    }
  }
}