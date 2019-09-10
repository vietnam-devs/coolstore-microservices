using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace VND.CoolStore.ShoppingCart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
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
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.MinRequestBodyDataRate = null;
                        options.ListenLocalhost(15003, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                        options.ListenLocalhost(5003, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
