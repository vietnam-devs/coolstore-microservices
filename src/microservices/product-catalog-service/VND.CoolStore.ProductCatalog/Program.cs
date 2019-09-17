using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace VND.CoolStore.ProductCatalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

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
                        var ipAddr = context.HostingEnvironment.IsDevelopment() ? IPAddress.Loopback : IPAddress.Parse("0.0.0.0");
                        options.Limits.MinRequestBodyDataRate = null;
                        options.Listen(ipAddr, 15002, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                        options.Listen(ipAddr, 5002, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddSerilog();
                });
    }
}
