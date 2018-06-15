using System;
using System.Collections.Generic;
using System.Net.Http;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.Swagger;
using VND.Services.Inventory.Infrastructure.Middlewares;
using VND.Services.Inventory.Infrastructure.Swagger;
using VND.Services.Inventory.UseCases.Service;
using VND.Services.Inventory.UseCases.Service.Impl;

namespace VND.Services.Inventory
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration = configuration;
			HostingEnvironment = env;
			IdentityModelEventSource.ShowPII = true;
		}

		public IConfiguration Configuration { get; }
		public IHostingEnvironment HostingEnvironment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var (authorityServer, _, __) = GetEnvironmentVariables();

			services.AddScoped<IInventoryService, InventoryService>();

			services.AddMvcCore().AddVersionedApiExplorer(
				options =>
				{
					options.GroupNameFormat = "'v'VVV";
					options.SubstituteApiVersionInUrl = true;
				});

			services.AddMvc();

			services.AddApiVersioning(o => o.ReportApiVersions = true);

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie()
				.AddIdentityServerAuthentication(c =>
				{
					c.Authority = authorityServer;
					c.RequireHttpsMetadata = false;
					c.SaveToken = true;
					c.ApiName = "inventory_api";
					c.JwtBackChannelHandler = new HttpClientHandler()
					{
						ServerCertificateCustomValidationCallback = 
							HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
					};
				});

			services.AddAuthorization(
				c => { c.AddPolicy("inventory_api_scope", p => p.RequireClaim("scope", "inventory_api_scope")); }
			);

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
						AuthorizationUrl = $"{authorityServer}/connect/authorize",
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
			var (_, basePath, currentHostUri) = GetEnvironmentVariables();

			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			
			if (!string.IsNullOrEmpty(basePath))
			{
				app.Use(async (context, next) =>
				{
					context.Request.PathBase = basePath;
					await next.Invoke();
				});
			}

			app.UseAuthentication();

			app.UseMiddleware<LoggingMiddleware>();

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
					c.OAuth2RedirectUrl($"{currentHostUri}/swagger/oauth2-redirect.html");
				});

			/*app.UseReDoc(c =>
            {
				var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                c.RoutePrefix = "api-docs";

				// build a swagger endpoint for each discovered API version
				foreach (var description in provider.ApiVersionDescriptions)
				{
					c.SpecUrl = $"{basePath}swagger/{description.GroupName}/swagger.json";
				}
            });*/
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

		private (string, string, string) GetEnvironmentVariables()
		{
			var authorityServer = Environment.GetEnvironmentVariable("AUTHORITY_HOST_URI");
			var basePath = Environment.GetEnvironmentVariable("ASPNETCORE_BASEPATH");
			var currentHostUri = Environment.GetEnvironmentVariable("CURRENT_HOST_URI");

			return (authorityServer, basePath, currentHostUri);
		}
	}
}
