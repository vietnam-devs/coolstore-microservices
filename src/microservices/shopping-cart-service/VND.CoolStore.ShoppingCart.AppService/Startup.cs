using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace VND.CoolStore.ShoppingCart.AppService
{
    using CloudNativeKit.Infrastructure;
    using CloudNativeKit.Infrastructure.Bus;
    using VND.CoolStore.ShoppingCart.Data;

    public class AppServiceStartupRoot { }
    public static class Startup
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddDataPersistence();
            services.AddDomainEventBus();
            return services.AddServiceByIntefaceInAssembly<AppServiceStartupRoot>(typeof(IValidator<>));
        }
    }
}
