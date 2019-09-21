using System.Reflection;
using CloudNativeKit.Infrastructure;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Data;
using CloudNativeKit.Infrastructure.ValidationModel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog
{
    public static class Startup
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<Product>(typeof(IValidator<>));
            services.AddDapperComponents();
            services.AddDomainEventBus();
            return services;
        }
    }
}
