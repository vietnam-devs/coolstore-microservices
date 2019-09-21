using System;
using System.Threading.Tasks;
using GrpcJsonTranscoder;
using GrpcJsonTranscoder.Grpc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using static VND.CoolStore.ProductCatalog.DataContracts.V1.Catalog;
using static VND.CoolStore.ShoppingCart.DataContracts.V1.ShoppingCart;

namespace VND.CoolStore.WebApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Log.Debug($"Root Cert content: {CertificateFactory.GetSslCredential().RootCertificates.ToString()}");
            //Log.Debug($"Cert Chain content: {CertificateFactory.GetSslCredential().KeyCertificatePair.CertificateChain.ToString()}");
            //Log.Debug($"Private Key content: {CertificateFactory.GetSslCredential().KeyCertificatePair.PrivateKey.ToString()}");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting host");
                await BuildWebHost(args).RunAsync();
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

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                         .AddJsonFile("ocelot.json")
                        .AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices(services =>
                {
                    services.AddGrpcJsonTranscoder(() =>
                        new GrpcAssemblyResolver().ConfigGrpcAssembly(
                            typeof(ShoppingCartClient).Assembly,
                            typeof(CatalogClient).Assembly));

                    services.AddOcelot();
                    services.AddHttpContextAccessor();
                })
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddSerilog(dispose: true);
                })
                .Configure(app =>
                {
                    var configuration = new OcelotPipelineConfiguration
                    {
                        PreQueryStringBuilderMiddleware = async (ctx, next) =>
                        {
                            await ctx.HandleGrpcRequestAsync(next/*, CertificateFactory.GetSslCredential()*/);
                        }
                    };

                    app.UseOcelot(configuration).Wait();
                })
                .Build();
    }
}
