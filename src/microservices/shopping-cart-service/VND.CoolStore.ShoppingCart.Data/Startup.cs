using CloudNativeKit.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace VND.CoolStore.ShoppingCart.Data
{
    public static class Startup
    {
        public static IServiceCollection AddDataPersistence(this IServiceCollection services)
        {
            services.AddEfSqlServerDb<ShoppingCartDataContext>(typeof(Startup).Assembly.GetName().Name);
            return services;
        }
    }
}
