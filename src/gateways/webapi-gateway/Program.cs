using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcJsonTranscoder;
using GrpcJsonTranscoder.Grpc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using static VND.CoolStore.Inventory.DataContracts.Api.V1.InventoryApi;
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
                            typeof(InventoryApiClient).Assembly,
                            typeof(CatalogApiClient).Assembly));

                    // only for demo
                    services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy",
                            builder => builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());
                    });

                    services.AddOcelot();
                    services.AddHttpContextAccessor();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddSerilog(dispose: true);
                })
                .Configure(app =>
                {
                    var configuration = new OcelotPipelineConfiguration
                    {
                        PreQueryStringBuilderMiddleware = async (ctx, next) =>
                        {
                            try
                            {
                                await ctx.HandleGrpcRequestAsync(next);
                            }
                            catch (Exception ex)
                            {
                                if (!(ex.InnerException is AggregateException innerEx))
                                    return;

                                innerEx.InnerExceptions
                                    .Select(async aggException =>
                                    {
                                        if (aggException is RpcException rpcException)
                                        {
                                            if (rpcException.StatusCode == StatusCode.Internal)
                                            {
                                                ctx.HttpContext.Response.StatusCode = 500;
                                                await ctx.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes($"{rpcException.Message}"));
                                            }
                                            else
                                            {
                                                var status = GetTrailerKeyOnRpcException(rpcException, ":status");
                                                var authMessage = GetTrailerKeyOnRpcException(rpcException, "www-authenticate");
                                                if (status == "401")
                                                {
                                                    ctx.HttpContext.Response.StatusCode = 401;
                                                    await ctx.HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes($"{authMessage}"));
                                                }
                                            }
                                        }
                                    })
                                    .ToList();

                                throw ex;
                            }
                        }
                    };

                    app.UseCors("CorsPolicy");
                    app.UseOcelot(configuration).Wait();
                })
                .Build();

        private static string GetTrailerKeyOnRpcException(RpcException rpcException, string key)
        {
            return rpcException.Trailers?.Select(x =>
            {
                if (x.Key == key)
                    return x.Value;
                return string.Empty;
            })
            .FirstOrDefault(x => !string.IsNullOrEmpty(x));
        }
    }
}
