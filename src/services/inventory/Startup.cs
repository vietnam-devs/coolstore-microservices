using System;
using System.Collections.Generic;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using VND.Services.Inventory.Swagger;
using VND.Services.Inventory.v1.Service;
using VND.Services.Inventory.v1.Service.Impl;

namespace VND.Services.Inventory
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
      services.AddScoped<IInventoryService, InventoryService>();

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie()
                  .AddIdentityServerAuthentication(c =>
                  {
                    c.Authority = "http://localhost:5000";
                    c.RequireHttpsMetadata = false;
                    c.ApiName = "inventory_api";
                  });

      services.AddAuthorization(
          c =>
            {
              c.AddPolicy("inventory_api_scope", p => p.RequireClaim("scope", "inventory_api_scope"));
            }
      );

      services.AddMvcCore().AddVersionedApiExplorer(
          options =>
          {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
          });

      services.AddMvc();

      services.AddApiVersioning(o => o.ReportApiVersions = true);

      services.AddSwaggerGen(
          c =>
          {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
              c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }

            // options.IncludeXmlComments (XmlCommentsFilePath);

            c.AddSecurityDefinition("oauth2", new OAuth2Scheme
            {
              Type = "oauth2",
              Flow = "implicit",
              AuthorizationUrl = "http://localhost:5000/connect/authorize",
              Scopes = new Dictionary<string, string>
                        {
                            {"inventory_api_scope", "Inventory APIs"}
                        }
            });

            c.OperationFilter<SecurityRequirementsOperationFilter>();
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

      string basePath = Environment.GetEnvironmentVariable("ASPNETCORE_BASEPATH");
      if (!string.IsNullOrEmpty(basePath))
      {
        app.Use(async (context, next) =>
        {
          context.Request.PathBase = basePath;
          await next.Invoke();
        });
      }

      app.UseAuthentication();

      app.UseMvc();

      app.UseSwagger();

      app.UseSwaggerUI(
          c =>
          {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            // build a swagger endpoint for each discovered API version
            foreach (var description in provider.ApiVersionDescriptions)
            {
              c.SwaggerEndpoint($"{basePath}swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
            }

            c.OAuthClientId("inventory_swagger_id");
            c.OAuthClientSecret("secret".Sha256());
            c.OAuthAppName("inventory_swagger_app");
            c.OAuth2RedirectUrl("http://localhost:51033/swagger/oauth2-redirect.html");
          });
    }

    static Info CreateInfoForApiVersion(ApiVersionDescription description)
    {
      var info = new Info()
      {
        Title = $"Sample API {description.ApiVersion}",
        Version = description.ApiVersion.ToString(),
        Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
        Contact = new Contact() { Name = "VN Devs", Email = "vietnam.devs.group@gmail.com" },
        TermsOfService = "Shareware",
        License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
      };

      if (description.IsDeprecated)
      {
        info.Description += " This API version has been deprecated.";
      }

      return info;
    }
  }
}
