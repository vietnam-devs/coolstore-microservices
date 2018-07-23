using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Service;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Service.Impl;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Swagger;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.AspNetCore.Middlewares;

namespace VND.CoolStore.Services.ApiGateway
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
      Configuration = configuration;
      Environment = env;
      IdentityModelEventSource.ShowPII = true;
    }

    public IConfiguration Configuration { get; }
    public IHostingEnvironment Environment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

      string authServerUri = Configuration.GetHostUri(Environment, "Auth");
      string externalAuthServerUri = Configuration.GetExternalHostUri("Auth");

      services.AddHttpContextAccessor();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
      services.AddScoped<IUrlHelper>(implementationFactory =>
      {
        ActionContext actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
        return new UrlHelper(actionContext);
      });
      services.AddSingleton(typeof(RestClient), typeof(RestClient));
      services.AddScoped<ICatalogService, CatalogService>();
      services.AddScoped<ICartService, CartService>();
      services.AddScoped<IInventoryService, InventoryService>();

      services.AddRouting(options => options.LowercaseUrls = true);
      services.AddMvcCore().AddVersionedApiExplorer(
          options =>
          {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
          });

      services.AddMvc()
          .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
          .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      services.AddApiVersioning(o =>
      {
        o.ReportApiVersions = true;
      });

      services
          .AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          .AddJwtBearer(options =>
          {
            options.Authority = authServerUri;
            options.RequireHttpsMetadata = false;
            options.Audience = "api";
          });

      services.AddAuthorization(
          c =>
          {
            c.AddPolicy("access_inventory_api", p => p.RequireClaim("scope", "inventory_api_scope"));
            c.AddPolicy("access_cart_api", p => p.RequireClaim("scope", "cart_api_scope"));
            c.AddPolicy("access_pricing_api", p => p.RequireClaim("scope", "pricing_api_scope"));
            c.AddPolicy("access_review_api", p => p.RequireClaim("scope", "review_api_scope"));
            c.AddPolicy("access_catalog_api", p => p.RequireClaim("scope", "catalog_api_scope"));
          }
      );

      services.AddSwaggerGen(c =>
      {
        IApiVersionDescriptionProvider provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

        c.DescribeAllEnumsAsStrings();

        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
          c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        // options.IncludeXmlComments (XmlCommentsFilePath);

        c.AddSecurityDefinition("oauth2", new OAuth2Scheme
        {
          Type = "oauth2",
          Flow = "implicit",
          AuthorizationUrl = $"{externalAuthServerUri}/connect/authorize",
          TokenUrl = $"{externalAuthServerUri}/connect/token",
          Scopes = new Dictionary<string, string>
              {
                        {"inventory_api_scope", "Inventory APIs"},
                        {"cart_api_scope", "Cart APIs"},
                        {"pricing_api_scope", "Pricing APIs"},
                        {"review_api_scope", "Review APIs"},
                        {"catalog_api_scope", "Catalog APIs"}
              }
        });

        c.EnableAnnotations();
        c.OperationFilter<SecurityRequirementsOperationFilter>();
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
      });

      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
                  policy => policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
      });

      services.AddHttpPolly();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      string basePath = Configuration.GetBasePath();
      string currentHostUri = Configuration.GetExternalCurrentHostUri();

      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
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

      if (!string.IsNullOrEmpty(basePath))
      {
        loggerFactory.CreateLogger("init").LogDebug($"Using PATH BASE '{basePath}'");
        app.Use(async (context, next) =>
        {
          context.Request.PathBase = basePath;
          await next.Invoke();
        });
      }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      app.Map("/liveness", lapp => lapp.Run(async ctx => ctx.Response.StatusCode = 200));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

      if (!env.IsDevelopment())
      {
        app.UseForwardedHeaders();
      }

      app.UseCors("CorsPolicy");
      app.UseAuthentication();

      app.UseMiddleware<LogHandlerMiddleware>();
      app.UseMiddleware<ErrorHandlerMiddleware>();

      app.UseMvc();
      app.UseSwagger();
      app.UseSwaggerUI(
          c =>
          {
            IApiVersionDescriptionProvider provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            // build a swagger endpoint for each discovered API version
            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
              c.SwaggerEndpoint($"{basePath}swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
            }

            c.OAuthClientId("swagger_id");
            c.OAuthClientSecret("secret".Sha256());
            c.OAuthAppName("swagger_app");
            c.OAuth2RedirectUrl($"{currentHostUri}/swagger/oauth2-redirect.html");
          });
    }

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
