using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudNativeKit.Infrastructure.Serilog;
using CloudNativeKit.Infrastructure.Tracing.Jaeger;
using CorrelationId;
using Grpc.Core;
using GrpcJsonTranscoder;
using GrpcJsonTranscoder.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OpenTracing.Contrib.Grpc.Interceptors;
using OpenTracing.Util;
using Serilog;
using static VND.CoolStore.Inventory.DataContracts.Api.V1.InventoryApi;
using static VND.CoolStore.ProductCatalog.DataContracts.Api.V1.CatalogApi;
using static VND.CoolStore.Search.DataContracts.Api.V1.ProductSearchApi;
using static VND.CoolStore.ShoppingCart.DataContracts.Api.V1.ShoppingCartApi;

namespace VND.CoolStore.WebApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Log.Information("Starting host");
                await BuildWebHost(args).Build().RunAsync();
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

        public static IHostBuilder BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
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
                        services.AddCorrelationId();
                        services.AddJaeger();

                        services.AddGrpcJsonTranscoder(() =>
                            new GrpcAssemblyResolver().ConfigGrpcAssembly(
                                typeof(ShoppingCartApiClient).Assembly,
                                typeof(InventoryApiClient).Assembly,
                                typeof(CatalogApiClient).Assembly,
                                typeof(ProductSearchApiClient).Assembly));

                        // only for demo
                        services.AddCors(options =>
                        {
                            options.AddPolicy("CorsPolicy",
                                builder => builder.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
                        });

                        services.AddOcelot();
                        services.AddHttpContextAccessor();
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        var seqUrl = hostingContext.Configuration.GetValue<string>("Seq:Connection");
                        Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment.EnvironmentName)
                            .Enrich.WithProperty("Microservices", "ApiGateway")
                            .Enrich.FromLogContext()
                            .Enrich.With<OpenTracingContextEnricher>()
                            .WriteTo.Console(Serilog.Events.LogEventLevel.Information, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{CorrelationID}] {Message}{NewLine}{Exception}")
                            .WriteTo.Seq(seqUrl)
                            .CreateLogger();

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
                                    var tracer = GlobalTracer.Instance;

                                    if (tracer?.ActiveSpan == null)
                                    {
                                        return;
                                    }

                                    await ctx.HandleGrpcRequestAsync(next, new[] { new ClientTracingInterceptor(tracer) });
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

                        app.UseCorrelationId();

                        app.UseOcelot(configuration).Wait();
                    });
                });

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
