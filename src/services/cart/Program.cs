using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using NetCoreKit.Infrastructure;
using NetCoreKit.Infrastructure.EfCore;
using NetCoreKit.Infrastructure.EfCore.Db;
using NetCoreKit.Infrastructure.EfCore.MySql;
using NetCoreKit.Infrastructure.GrpcHost;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Gateways;
using VND.CoolStore.Services.Cart.v1.Db;

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
                        services.AddEfCoreMySqlDb();
                    },
                    svc =>
                    {
                        IdentityModelEventSource.ShowPII = true;
                        svc.AddHostedService<HostedService>();
                        svc.AddScoped<ICatalogGateway, CatalogGateway>();
                        svc.AddScoped<IPromoGateway, PromoGateway>();
                        svc.AddScoped<IShippingGateway, ShippingGateway>();
                    });

            await host.RunAsync();
        }
    }
}
