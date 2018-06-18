// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VND.CoolStore.Services.Idp.Certificate;

namespace IdentityServer4
{
	public class Startup
	{
		public IHostingEnvironment Environment { get; }
		public IConfiguration Configuration { get; }

		public Startup(IHostingEnvironment environment, IConfiguration configuration)
		{
			Environment = environment;
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.Configure<IISOptions>(options =>
			{
				options.AutomaticAuthentication = false;
				options.AuthenticationDisplayName = "Windows";
			});

			if (Environment.IsDevelopment())
			{
				services.AddHttpsRedirection(options =>
				{
					options.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
					options.HttpsPort = 5001; // TODO: hard code
				});
			}

			var builder = services.AddIdentityServer(options =>
			  {
				  options.Events.RaiseErrorEvents = true;
				  options.Events.RaiseInformationEvents = true;
				  options.Events.RaiseFailureEvents = true;
				  options.Events.RaiseSuccessEvents = true;
			  })
			  .AddTestUsers(TestUsers.Users)
			.AddJwtBearerClientAuthentication();

			// in-memory, code config
			var clients = Config.GetClients().ToList();

			// get swagger and process it
			var hostSettings = Configuration.GetSection("HostSettings");
			if (hostSettings != null)
			{
				clients[0].RedirectUris.Add(hostSettings.GetValue<string>("SwaggerRedirectUri"));
				clients[0].PostLogoutRedirectUris.Add(hostSettings.GetValue<string>("SwaggerPostLogoutRedirectUri"));
				clients[0].AllowedCorsOrigins.Add(hostSettings.GetValue<string>("SwaggerAllowedCorsOrigin"));
			}

			builder.AddInMemoryIdentityResources(Config.GetIdentityResources());
			builder.AddInMemoryApiResources(Config.GetApis());
			builder.AddInMemoryClients(clients);

			// in-memory, json config
			// builder.AddInMemoryIdentityResources(Configuration.GetSection("IdentityResources"));
			// builder.AddInMemoryApiResources(Configuration.GetSection("ApiResources"));
			// builder.AddInMemoryClients(Configuration.GetSection("clients"));

			builder.AddDeveloperSigningCredential();

			if (Environment.IsDevelopment())
			{
				builder.AddSigningCredential(Certificate.Get());
			}

			services.AddAuthentication()
			  .AddGoogle(options =>
			  {
				  options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

				  options.ClientId = "708996912208-9m4dkjb5hscn7cjrn5u0r4tbgkbj1fko.apps.googleusercontent.com";
				  options.ClientSecret = "wdfPY6t8H8cecgjlxud__4Gh";
			  });
		}

		public void Configure(IApplicationBuilder app)
		{
			if (Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseHsts();
			}

			string basePath = System.Environment.GetEnvironmentVariable("ASPNETCORE_BASEPATH");
			if (!string.IsNullOrEmpty(basePath))
			{
				app.Use(async (context, next) =>
				{
					context.Request.PathBase = basePath;
					await next.Invoke();
				});
			}

			if (!Environment.IsDevelopment())
			{
				var fordwardedHeaderOptions = new ForwardedHeadersOptions
				{
					ForwardedHeaders = ForwardedHeaders.XForwardedFor |
					                   ForwardedHeaders.XForwardedProto,
				};

				fordwardedHeaderOptions.KnownNetworks.Clear();
				fordwardedHeaderOptions.KnownProxies.Clear();
				app.UseForwardedHeaders(fordwardedHeaderOptions);
			}

			if (Environment.IsDevelopment())
			{
				app.UseHttpsRedirection();
			}

			app.UseIdentityServer();
			app.UseStaticFiles();
			app.UseMvcWithDefaultRoute();
		}
	}
}
