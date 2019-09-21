using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace VND.CoolStore.ProductCatalog.Grpc
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
                        IdentityModelEventSource.ShowPII = true; // only for demo

                        //var cert = CertificateFactory.GetServerPfx();
                        //Log.Debug($"Cert content: {cert.ToString()}");
                        options.Limits.MinRequestBodyDataRate = null;

                        options.Listen(IPAddress.Any, 15002, listenOptions => {
                            //listenOptions.UseHttps(cert);
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });

                        /*options.ConfigureHttpsDefaults(httpsOptions =>
                        {
                            httpsOptions.ServerCertificate = cert;
                            httpsOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                        });*/
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddSerilog();
                });
    }
}
