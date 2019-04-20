using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreKit.Infrastructure.GrpcHost;
using NetCoreKit.Utils.Extensions;
using VND.CoolStore.Services.Cart.v1.Grpc;
using VND.CoolStore.Services.Cart.v1.Services;

namespace VND.CoolStore.Services.Cart
{
    public class HostedService : HostedServiceBase
    {
        private readonly IServiceProvider _resolver;

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

            if (!env.IsDevelopment())
            {
                port = Environment.GetEnvironmentVariable($"{serviceName.ToUpperInvariant()}_HOST").ConvertTo<int>();
            }

            var server = new Server
            {
                Services =
                {
                    CartService.BindService(new CartServiceImpl(_resolver)).Intercept(new AuthNInterceptor(_resolver)),
                    Grpc.Health.V1.Health.BindService(new HealthImpl())
                },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
            };

            Logger.LogInformation($"{nameof(CartService)} is listening on {host}:{port}.");
            return server;
        }

        protected override void SuppressFinalize()
        {
        }
    }
}
