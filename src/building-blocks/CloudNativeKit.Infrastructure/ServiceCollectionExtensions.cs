using System;
using Microsoft.Extensions.DependencyInjection;

namespace CloudNativeKit.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceByIntefaceInAssembly<TRegisteredAssemblyType>(this IServiceCollection services, Type interfaceType)
        {
            services.Scan(s =>
                s.FromAssemblyOf<TRegisteredAssemblyType>()
                    .AddClasses(c => c.AssignableTo(interfaceType))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            return services;
        }
    }
}
