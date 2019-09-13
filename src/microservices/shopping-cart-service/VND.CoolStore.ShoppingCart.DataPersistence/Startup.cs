using CloudNativeKit.Infrastructure.DataPersistence.EfCore;
using CloudNativeKit.Infrastructure.DataPersistence.EfCore.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VND.CoolStore.ShoppingCart.DataPersistence
{
    public static class Startup
    {
        public static IServiceCollection AddDataPersistence(this IServiceCollection services)
        {
            services.AddInMemoryDb();

            services.AddDbContext<ShoppingCartDataContext>((sp, o) =>
            {
                using var scope = sp.CreateScope();
                var resolver = scope.ServiceProvider;

                var extendOptionsBuilder = resolver.GetRequiredService<IExtendDbContextOptionsBuilder>();
                var connStringFactory = resolver.GetRequiredService<IDbConnStringFactory>();

                extendOptionsBuilder.Extend(o, connStringFactory, typeof(Startup).Assembly.GetName().Name);
            });

            services.AddScoped<DbContext>(resolver => resolver.GetService<ShoppingCartDataContext>());
            services.AddGenericRepository();

            return services;
        }
    }
}
