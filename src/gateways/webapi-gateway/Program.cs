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
using static VND.CoolStore.ProductCatalog.DataContracts.Api.V1.CatalogApi;
using static VND.CoolStore.ShoppingCart.DataContracts.Api.V1.ShoppingCartApi;

namespace VND.CoolStore.WebApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
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
                            typeof(ShoppingCartApiClient).Assembly,
                            typeof(CatalogApiClient).Assembly));

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
                        PreQueryStringBuilderMiddleware = async (ctx, next) => await ctx.HandleGrpcRequestAsync(next)
                    };

                    app.UseOcelot(configuration).Wait();
                })
                .Build();
    }
}
