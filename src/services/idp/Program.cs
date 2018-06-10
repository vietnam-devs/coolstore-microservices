// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using VND.Services.Idp.Certificate;

namespace Idp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.Title = "IdentityServer4";

			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
					/*.UseKestrel(options =>
					{
						options.AddServerHeader = false;

						// listen for HTTP
						options.Listen(IPAddress.Loopback, 5000);

						// listen for HTTPS
						options.Listen(IPAddress.Loopback, 5001, listenOptions =>
						{
							// var byteCert = Convert.FromBase64String("coolstore.pfx");
							// listenOptions.UseHttps(new X509Certificate2(byteCert, "vietnam"));
							var cert = new X509Certificate2("coolstore.pfx", "vietnam");
							listenOptions.UseHttps(cert);
						});
					})*/
					.UseStartup<Startup>()
					.UseSerilog((context, configuration) =>
					{
						configuration
							.MinimumLevel.Debug()
							.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
							.MinimumLevel.Override("System", LogEventLevel.Warning)
							.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
							.Enrich.FromLogContext()
							.WriteTo.File(@"identityserver4_log.txt")
							.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
					})
					.Build();
		}
	}
}
