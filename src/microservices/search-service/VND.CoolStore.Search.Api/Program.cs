using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using CloudNativeKit.Infrastructure.Serilog;

namespace VND.CoolStore.Search.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        IdentityModelEventSource.ShowPII = true; // only for demo
                        options.Limits.MinRequestBodyDataRate = null;

                        options.Listen(IPAddress.Any, 5010);
                        options.Listen(IPAddress.Any, 15010, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });

                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    var seqUrl = hostingContext.Configuration.GetValue<string>("Seq:Connection");

                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName)
                        .Enrich.WithProperty("Microservices", "SearchService")
                        .Enrich.FromLogContext()
                        .Enrich.With<OpenTracingContextEnricher>()
                        .WriteTo.Console(Serilog.Events.LogEventLevel.Information, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{CorrelationID}] {Message}{NewLine}{Exception}")
                        .WriteTo.Seq(seqUrl)
                        .CreateLogger();

                    logging.AddSerilog(dispose: true);
                });
    }
}
