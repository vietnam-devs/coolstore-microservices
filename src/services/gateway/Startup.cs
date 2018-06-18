using System;
using System.Collections.Generic;
using System.Net.Http;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.Swagger;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Middlewares;
using VND.CoolStore.Services.ApiGateway.Infrastructure.Swagger;

namespace VND.CoolStore.Services.ApiGateway
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

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var (authorityServer, _, _) = GetEnvironmentVariables();

			services.AddRouting(options => options.LowercaseUrls = true);
			services.AddMvcCore().AddVersionedApiExplorer(
				options =>
				{
					options.GroupNameFormat = "'v'VVV";
					options.SubstituteApiVersionInUrl = true;
				});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddHttpContextAccessor();
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			services.AddScoped<IUrlHelper>(implementationFactory =>
			{
				var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
				return new UrlHelper(actionContext);
			});

			services.AddApiVersioning(o =>
			{
				o.ReportApiVersions = true;
				// o.ApiVersionReader = new HeaderApiVersionReader("api-version");
			});

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie()
				.AddIdentityServerAuthentication(c =>
				{
					c.Authority = authorityServer;
					c.RequireHttpsMetadata = false;
					c.SaveToken = true;
					c.ApiName = "api";
					c.JwtBackChannelHandler = new HttpClientHandler()
					{
						ServerCertificateCustomValidationCallback =
							HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
					};
				});

			services.AddAuthorization(
				c =>
				{
					c.AddPolicy("access_inventory_api", p => p.RequireClaim("scope", "inventory_api_scope"));
					c.AddPolicy("access_cart_api", p => p.RequireClaim("scope", "cart_api_scope"));
					c.AddPolicy("access_pricing_api", p => p.RequireClaim("scope", "pricing_api_scope"));
					c.AddPolicy("access_review_api", p => p.RequireClaim("scope", "review_api_scope"));
				}
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
							{"inventory_api_scope", "Inventory APIs"},
							{"cart_api_scope", "Cart APIs"},
							{"pricing_api_scope", "Pricing APIs"},
							{"review_api_scope", "Review APIs"}
						}
					});

					c.OperationFilter<SecurityRequirementsOperationFilter>();
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			var (_, currentHostUri, basePath) = GetEnvironmentVariables();

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

					c.OAuthClientId("swagger_id");
					c.OAuthClientSecret("secret".Sha256());
					c.OAuthAppName("swagger_app");
					c.OAuth2RedirectUrl($"{currentHostUri}/swagger/oauth2-redirect.html");
				});
		}

		static Info CreateInfoForApiVersion(ApiVersionDescription description)
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

		private (string, string, string) GetEnvironmentVariables()
		{
			var basePath = Environment.GetEnvironmentVariable("ASPNETCORE_BASEPATH");

			var config = Configuration.GetSection("HostSettings");
			var currentHostUri = config.GetValue<string>("CurrentHostUri");
			var authorityServer = config.GetValue<string>("AuthorityHostUri");

			return (authorityServer, currentHostUri, basePath);
		}
	}
}
