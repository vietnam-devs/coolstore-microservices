using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreKit.Infrastructure.GrpcHost;

namespace VND.CoolStore.Services.Inventory
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
                        /*services.AddEfCoreMySqlDb();
                        services.AddDbContext<InventoryDbContext>((sp, o) =>
                        {
                            var config = sp.GetService<IConfiguration>();
                            var extendOptionsBuilder = sp.GetRequiredService<IExtendDbContextOptionsBuilder>();
                            var connStringFactory = sp.GetRequiredService<IDatabaseConnectionStringFactory>();
                            extendOptionsBuilder.Extend(o, connStringFactory,
                                config.LoadApplicationAssemblies().FirstOrDefault()?.GetName().Name);
                        });

                        services.AddScoped<DbContext>(resolver => resolver.GetService<InventoryDbContext>());
                        services.AddGenericRepository();*/
                    },
                    services =>
                    {
                        services.AddHostedService<HostedService>();
                    });

            await host.RunAsync();
        }
    }
}
