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
using System.Text.RegularExpressions;
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
      var config = serviceProvider.GetRequiredService<IConfiguration>();
      var env = serviceProvider.GetRequiredService<IHostingEnvironment>();

      var extendOptionsBuilder = serviceProvider.GetRequiredService<IExtendDbContextOptionsBuilder>();
      var dbConnectionStringFactory = serviceProvider.GetRequiredService<IDatabaseConnectionStringFactory>();
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

      services.AddApiVersioning(o => {
        o.ReportApiVersions = true;
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.DefaultApiVersion = ParseApiVersion(config.GetValue<string>("API_VERSION"));
      });

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

        services.AddAuthorization(c => configureAuthZ?.Invoke(c));
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
              Scopes = swaggerOauthSchemes?.Invoke(),
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

      app.Use(async (context, next) =>
      {
        // context.Request.Path = $"api/{version}/{controller}/ToDoItemDto/{remainingPath}";
        await next.Invoke();
      });

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

      app.UseMvc();

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

    private static ApiVersion ParseApiVersion(string serviceVersion)
    {
      if (string.IsNullOrEmpty(serviceVersion))
      {
        throw new Exception("[CS] ServiceVersion is null or empty.");
      }

      var pattern = @"(.)|(-)";
      var results = Regex.Split(serviceVersion, pattern)
        .Where(x => x != string.Empty && x != "." && x != "-")
        .ToArray();

      if(results == null || results.Count() < 2)
      {
        throw new Exception("[CS] Could not parse ServiceVersion.");
      }

      if(results.Count() > 2)
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
