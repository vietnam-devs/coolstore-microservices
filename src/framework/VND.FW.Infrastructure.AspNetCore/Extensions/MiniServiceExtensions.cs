// Reference at https://thenewstack.io/miniservices-a-realistic-alternative-to-microservices

using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using VND.FW.Infrastructure.AspNetCore.Middlewares;
using VND.FW.Infrastructure.AspNetCore.Swagger;
using VND.FW.Infrastructure.EfCore.Db;
using VND.FW.Infrastructure.EfCore.Extensions;
using VND.FW.Infrastructure.EfCore.Options;

namespace VND.FW.Infrastructure.AspNetCore.Extensions
{
  public static class MiniServiceExtensions
  {
    public static IServiceCollection AddMiniService<TDbContext>(this IServiceCollection services,
      Assembly startupAssembly,
      Action<AuthorizationOptions> configureAuthZ = null,
      Func<Dictionary<string, string>> swaggerOauthSchemes = null)
      where TDbContext : DbContext
    {
      var serviceProvider = services.BuildServiceProvider();
      var config = serviceProvider.GetService<IConfiguration>();
      var env = serviceProvider.GetService<IHostingEnvironment>();
      var extendOptionsBuilder = serviceProvider.GetService<IExtendDbContextOptionsBuilder>();
      var dbConnectionStringFactory = serviceProvider.GetService<IDatabaseConnectionStringFactory>();

      IdentityModelEventSource.ShowPII = true;

      if (config.GetValue("EnableAuthN", false))
      {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
      }

      services.AddOptions()
          .Configure<PersistenceOption>(config.GetSection("EfCore"));

      void optionsBuilderAction(DbContextOptionsBuilder optionsBuilder)
      {
        extendOptionsBuilder.Extend(
            optionsBuilder,
            dbConnectionStringFactory,
            startupAssembly.GetName().Name);
      }

      services.AddDbContext<TDbContext>(options => optionsBuilderAction(options));
      services.AddScoped<DbContext>(resolver => resolver.GetRequiredService<TDbContext>());

      services.AddEfCore();
      
      services.AddHttpContextAccessor();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
      services.AddScoped<IUrlHelper>(implementationFactory =>
      {
        var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
        return new UrlHelper(actionContext);
      });
      services.AddHttpClient<RestClient>();

      services.AddRouting(options => options.LowercaseUrls = true);
      services.AddMvcCore().AddVersionedApiExplorer(
        options =>
        {
          options.GroupNameFormat = "'v'VVV";
          options.SubstituteApiVersionInUrl = true;
        });

      services
        .AddMvc()
        .AddJsonOptions(options =>
          options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      services.AddApiVersioning(o => o.ReportApiVersions = true);

      if (config.GetValue("EnableAuthN", false))
      {
        services
          .AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          .AddJwtBearer(options =>
          {
            options.Authority = GetAuthUri(config, env);
            options.RequireHttpsMetadata = false;
            options.Audience = "api";
          });

        services.AddAuthorization(
            c =>
            {
              configureAuthZ?.Invoke(c);
              //c.AddPolicy("access_inventory_api", p => p.RequireClaim("scope", "inventory_api_scope"));
              //c.AddPolicy("access_cart_api", p => p.RequireClaim("scope", "cart_api_scope"));
              //c.AddPolicy("access_pricing_api", p => p.RequireClaim("scope", "pricing_api_scope"));
              //c.AddPolicy("access_review_api", p => p.RequireClaim("scope", "review_api_scope"));
              //c.AddPolicy("access_catalog_api", p => p.RequireClaim("scope", "catalog_api_scope"));
            }
        );
      }

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

          // options.IncludeXmlComments (XmlCommentsFilePath);

          if (config.GetValue("EnableAuthN", false))
          {
            c.AddSecurityDefinition("oauth2", new OAuth2Scheme
            {
              Type = "oauth2",
              Flow = "implicit",
              AuthorizationUrl = $"{GetExternalAuthUri(config)}/connect/authorize",
              TokenUrl = $"{GetExternalAuthUri(config)}/connect/token",
              Scopes = swaggerOauthSchemes?.Invoke()
              /*Scopes = new Dictionary<string, string>
              {
                {"inventory_api_scope", "Inventory APIs"},
                {"cart_api_scope", "Cart APIs"},
                {"pricing_api_scope", "Pricing APIs"},
                {"review_api_scope", "Review APIs"},
                {"catalog_api_scope", "Catalog APIs"}
              }*/
            });
          }

          c.EnableAnnotations();

          if (config.GetValue("EnableAuthN", false))
          {
            c.OperationFilter<SecurityRequirementsOperationFilter>();
          }

          c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });
      }

      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
                  policy => policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
      });

      if (!env.IsDevelopment())
      {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
          options.ForwardedHeaders =
              ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
      }

      return services;
    }

    public static IApplicationBuilder UseMiniService(this IApplicationBuilder app)
    {
      var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
      var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
      var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger("init");

      var basePath = config.GetBasePath();
      var currentHostUri = config.GetExternalCurrentHostUri();

      loggerFactory.AddConsole(config.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      app.Map("/liveness", lapp => lapp.Run(async ctx => ctx.Response.StatusCode = 200));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

      if (!string.IsNullOrEmpty(basePath))
      {
        logger.LogInformation($"Using PATH BASE '{basePath}'");
        app.UsePathBase(basePath);
      }

      if (!env.IsDevelopment())
      {
        app.UseForwardedHeaders();
      }

      app.UseMiddleware<LogHandlerMiddleware>();
      app.UseMiddleware<ErrorHandlerMiddleware>();

      app.UseCors("CorsPolicy");

      if (config.GetValue("EnableAuthN", false))
      {
        app.UseAuthentication();
      }

      app.UseMvcWithDefaultRoute();

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
          });
      }

      return app;
    }

    private static string GetAuthUri(IConfiguration config, IHostingEnvironment env)
    {
      return config.GetHostUri(env, "Auth");
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
      Info info = new Info()
      {
        Title = $"API {description.ApiVersion}",
        Version = description.ApiVersion.ToString(),
        Description = "An application with Swagger, Swashbuckle, and API versioning.",
        Contact = new Contact() { Name = "VND", Email = "vietnam.devs.group@gmail.com" },
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
