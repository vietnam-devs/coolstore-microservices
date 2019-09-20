using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
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
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        var cert = new X509Certificate2(Path.Combine("Certs\\server.pfx"), "1111");
                        //var ipAddr = context.HostingEnvironment.IsDevelopment() ? IPAddress.Loopback : IPAddress.Parse("0.0.0.0");

                        options.Limits.MinRequestBodyDataRate = null;

                        //TODO: move http request into another host
                        options.ListenAnyIP(5003, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1;
                        });

                        options.ListenAnyIP(15003, listenOptions =>
                        {
                            listenOptions.UseHttps(cert);
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });

                        options.ConfigureHttpsDefaults(httpsOptions =>
                        {
                            httpsOptions.ServerCertificate = cert;
                            httpsOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
