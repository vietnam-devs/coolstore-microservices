using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using NetCoreKit.GrpcTemplate.MongoDb;
using NetCoreKit.Infrastructure.GrpcHost;
using NetCoreKit.Utils.Extensions;
using VND.CoolStore.Services.Review.v1.Grpc;
using VND.CoolStore.Services.Review.v1.Services;

namespace VND.CoolStore.Services.Review
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureDefaultSettings(
                    args, svc => { svc.AddHostedService<HostedService>(); });
            await host.RunAsync();
        }
    }

    public class HostedService : HostedServiceBase
    {
        private readonly IServiceProvider _resolver;
        private Server _server;

        public HostedService(IServiceProvider resolver)
            : base(
                resolver.GetRequiredService<ILoggerFactory>(),
                resolver.GetRequiredService<IApplicationLifetime>(),
                resolver.GetRequiredService<IConfiguration>())
        {
            _resolver = resolver;
        }

        protected override Server ConfigureServer()
        {
            GrpcEnvironment.SetLogger(new ConsoleLogger());

            var env = _resolver.GetRequiredService<IHostingEnvironment>();
            var host = Config["Hosts:Local:Host"];
            var port = Config["Hosts:Local:Port"].ConvertTo<int>();
            var serviceName = Config["Hosts:ServiceName"];

            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            if (!env.IsDevelopment())
            {
                port = Environment.GetEnvironmentVariable($"{serviceName.ToUpperInvariant()}_HOST").ConvertTo<int>();
            }

            _server = new Server
            {
                Services =
                {
                    ReviewService.BindService(new ReviewServiceImpl(_resolver)).Intercept(new AuthNInterceptor(_resolver)),
                    PingService.BindService(new PingServiceImpl(_resolver)).Intercept(new AuthNInterceptor(_resolver)),
                    Grpc.Health.V1.Health.BindService(new HealthImpl())
                },
                Ports = {new ServerPort(host, port, ServerCredentials.Insecure)}
            };

            Logger.LogInformation($"{nameof(ReviewService)} is listening on {host}:{port}.");
            return _server;
        }

        protected override void SuppressFinalize()
        {
            if (_server != null)
            {
                GC.SuppressFinalize(_server);
            }
        }
    }
}
