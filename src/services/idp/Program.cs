// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

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
					.UseKestrel()
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
