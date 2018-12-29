using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.Host.gRPC;
using NetCoreKit.Template.gRPC.MongoDb;
using VND.CoolStore.Services.Review.v1.Grpc;
using VND.CoolStore.Services.Review.v1.Services;

namespace VND.CoolStore.Services.Review
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureDefaultSettings(args, svc =>
                {
                    svc.AddHostedService<HostedService>();
                });
            await host.RunAsync();
        }
    }

    public class HostedService : HostedServiceBase
    {
        private readonly IQueryRepositoryFactory _repositoryFactory;
        private readonly IUnitOfWorkAsync _uow;
        private readonly IHostingEnvironment _env;

        public HostedService(
            IQueryRepositoryFactory repositoryFactory,
            IUnitOfWorkAsync uow,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime,
            IConfiguration config,
            IHostingEnvironment env)
            : base(loggerFactory, appLifetime, config)
        {
            _repositoryFactory = repositoryFactory;
            _uow = uow;
            _env = env;
        }

        protected override Server ConfigureServer()
        {
            var host = Config["Hosts:Local:Host"];
            var port = int.Parse(Config["Hosts:Local:Port"]);

            if (!_env.IsDevelopment())
            {
                port = Convert.ToInt32(Environment.GetEnvironmentVariable("EXCHANGE_SERVICE_HOST"));
            }

            var server = new Server
            {
                Services =
                {
                    ReviewService.BindService(
                        new ReviewServiceImpl(
                            _repositoryFactory,
                            _uow,
                            LoggerFactory
                        )),
                    Grpc.Health.V1.Health.BindService(new HealthImpl())
                },
                Ports = {new ServerPort(host, port, ServerCredentials.Insecure)}
            };

            Logger.LogInformation($"{nameof(ReviewService)} is listening on {host}:{port}.");
            return server;
        }

        protected override void SuppressFinalize()
        {
        }
    }
}
