using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreKit.Infrastructure;
using NetCoreKit.Infrastructure.EfCore;
using NetCoreKit.Infrastructure.EfCore.Db;
using NetCoreKit.Infrastructure.EfCore.MySql;
using NetCoreKit.Infrastructure.GrpcHost;
using VND.CoolStore.Services.Inventory.v1.Db;

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
                        services.AddDbContext<InventoryDbContext>((sp, o) =>
                        {
                            var config = sp.GetService<IConfiguration>();
                            var extendOptionsBuilder = sp.GetRequiredService<IExtendDbContextOptionsBuilder>();
                            var connStringFactory = sp.GetRequiredService<IDatabaseConnectionStringFactory>();
                            extendOptionsBuilder.Extend(o, connStringFactory,
                                config.LoadApplicationAssemblies().FirstOrDefault()?.GetName().Name);
                        });

                        services.AddScoped<DbContext>(resolver => resolver.GetService<InventoryDbContext>());
                        services.AddGenericRepository();
                        services.AddEfCoreMySqlDb();
                    },
                    async services =>
                    {
                        services.AddHostedService<HostedService>();

                        // ensure running the first migration
                        using (var scope = services.BuildServiceProvider().CreateScope())
                        {
                            var revolver = scope.ServiceProvider;
                            var dbContext = revolver.GetService<InventoryDbContext>();
                            await dbContext?.Database?.MigrateAsync();
                        }
                    });

            await host.RunAsync();
        }
    }
}
