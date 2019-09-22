using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CloudNativeKit.Infrastructure.ValidationModel;
using CloudNativeKit.Infrastructure;
using CloudNativeKit.Infrastructure.Data;
using CloudNativeKit.Infrastructure.Bus;
using VND.CoolStore.ShoppingCart.Data;
using FluentValidation;
using VND.CoolStore.ShoppingCart.Domain.Cart;

namespace VND.CoolStore.ShoppingCart
{
    public static class Startup
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<Cart>(typeof(IValidator<>));
            services.AddEfSqlServerDb<ShoppingCartDataContext>(typeof(Startup).Assembly.GetName().Name);
            services.AddDomainEventBus();
            return services;
        }
    }
}
