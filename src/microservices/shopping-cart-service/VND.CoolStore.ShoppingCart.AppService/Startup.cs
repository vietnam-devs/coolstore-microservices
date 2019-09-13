using CloudNativeKit.Infrastructure.Bus;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.ShoppingCart.DataPersistence;

namespace VND.CoolStore.ShoppingCart.AppService
{
    public class AppServiceStartupRoot { }
    public static class Startup
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddDataPersistence();
            services.AddDomainEventBus();

            return services.Scan(s =>
                s.FromAssemblyOf<AppServiceStartupRoot>()
                    .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }
    }
}
