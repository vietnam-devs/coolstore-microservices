// Reference at https://thenewstack.io/miniservices-a-realistic-alternative-to-microservices

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using IdentityServer4.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using VND.FW.Infrastructure.AspNetCore.Middlewares;
using VND.FW.Infrastructure.AspNetCore.Swagger;
using VND.FW.Infrastructure.AspNetCore.Validation;
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
      var svcProvider = services.BuildServiceProvider();
      var config = svcProvider.GetRequiredService<IConfiguration>();
      var env = svcProvider.GetRequiredService<IHostingEnvironment>();

      var extendOptionsBuilder = svcProvider.GetRequiredService<IExtendDbContextOptionsBuilder>();
      var connStringFactory = svcProvider.GetRequiredService<IDatabaseConnectionStringFactory>();

      IdentityModelEventSource.ShowPII = true;

      if (config.GetValue("EnableAuthN", false))
      {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
      }

      services.AddOptions()
          .Configure<PersistenceOption>(config.GetSection("EfCore"));

      void OptionsBuilderAction(DbContextOptionsBuilder o)
      {
        extendOptionsBuilder.Extend(o, connStringFactory, startupAssembly.GetName().Name);
      }

      services.AddDbContextPool<TDbContext>(OptionsBuilderAction);
      services.AddSingleton<TDbContext>();
      services.AddSingleton<DbContext>(resolver => resolver.GetRequiredService<TDbContext>());
      services.AddEfCore();

      services.AddHttpContextAccessor();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
      services.AddSingleton<IUrlHelper>(factory =>
      {
        var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
        return new UrlHelper(actionContext);
      });
      services.AddHttpPolly<RestClient>();

      // MediatR
      services.AddMediatR(typeof(MiniServiceExtensions).Assembly, startupAssembly);
      services.Scan(
        scanner => scanner
          .FromAssemblies(startupAssembly)
          .AddClasses()
          .AsImplementedInterfaces());

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

      services
        .AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      services.Configure<ApiBehaviorOptions>(options =>
      {
        options.InvalidModelStateResponseFactory = ctx => new ValidationProblemDetailsResult();
      });

      /*services
        .AddMiniProfiler(o =>
        {
          o.RouteBasePath = "/profiler";
        });*/

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

          // c.IncludeXmlComments (XmlCommentsFilePath);

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
          options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
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
        // app.UseMiniProfiler();
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

      app.UseExceptionHandler(errorApp =>
      {
#pragma warning disable CS1998
        errorApp.Run(async context =>
        {
          var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
          var exception = errorFeature.Error;

          // the IsTrusted() extension method doesn't exist and
          // you should implement your own as you may want to interpret it differently
          // i.e. based on the current principal

          var problemDetails = new ProblemDetails
          {
            Instance = $"urn:myorganization:error:{Guid.NewGuid()}"
          };

          if (exception is BadHttpRequestException badHttpRequestException)
          {
            problemDetails.Title = "Invalid request";
            problemDetails.Status = (int)typeof(BadHttpRequestException).GetProperty(
              "StatusCode", BindingFlags.NonPublic | BindingFlags.Instance)
              ?.GetValue(badHttpRequestException);
            problemDetails.Detail = badHttpRequestException.Message;
          }
          else
          {
            problemDetails.Title = "An unexpected error occurred!";
            problemDetails.Status = 500;
            problemDetails.Detail = exception.Demystify().ToString();
          }

          // TODO: log the exception etc..

          context.Response.StatusCode = problemDetails.Status.Value;
          context.Response.WriteJson(problemDetails, "application/problem+json");
        }
#pragma warning restore CS1998
        );
      });

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

            // c.IndexStream = () => typeof(MiniServiceExtensions).GetTypeInfo().Assembly.GetManifestResourceStream("VND.FW.Infrastructure.AspNetCore.Swagger.index.html");
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
