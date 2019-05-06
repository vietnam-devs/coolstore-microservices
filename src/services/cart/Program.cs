using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreKit.Infrastructure;
using NetCoreKit.Infrastructure.EfCore;
using NetCoreKit.Infrastructure.EfCore.Db;
using NetCoreKit.Infrastructure.EfCore.MySql;
using NetCoreKit.Infrastructure.GrpcHost;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.v1.Db;
using static Coolstore.CatalogService;

namespace VND.CoolStore.Services.Cart
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureDefaultSettings(
                    args,
                    services =>
                    {
                        services.AddEfCoreMySqlDb();
                        services.AddDbContext<CartDbContext>((sp, o) =>
                        {
                            var config = sp.GetService<IConfiguration>();
                            var extendOptionsBuilder = sp.GetRequiredService<IExtendDbContextOptionsBuilder>();
                            var connStringFactory = sp.GetRequiredService<IDatabaseConnectionStringFactory>();
                            extendOptionsBuilder.Extend(o, connStringFactory,
                                config.LoadApplicationAssemblies().FirstOrDefault()?.GetName().Name);
                        });

                        services.AddScoped<DbContext>(resolver => resolver.GetService<CartDbContext>());
                        services.AddGenericRepository();
                    },
                    services =>
                    {
                        services.AddHostedService<HostedService>();
                        services.AddScoped<ICatalogGateway, CatalogGateway>();
                        services.AddScoped<IPromoGateway, PromoGateway>();
                        services.AddScoped<IShippingGateway, ShippingGateway>();
                        services.AddSingleton(typeof(CatalogServiceClient), RegisterGrpcService<CatalogServiceClient>(services, "CatalogEndPoint"));
                    });

            await host.RunAsync();
        }

        private static TService RegisterGrpcService<TService>(IServiceCollection services, string serviceName)
            where TService : ClientBase<TService>
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var rpcClients = config.GetSection("GrpcEndPoints");
                var channel = new Channel(rpcClients[serviceName], ChannelCredentials.Insecure);
                var client = (TService)typeof(TService)
                    .GetConstructor(new[] { typeof(Channel) })
                    .Invoke(new object[] { channel });

                return client;
            }
        }
    }
}
