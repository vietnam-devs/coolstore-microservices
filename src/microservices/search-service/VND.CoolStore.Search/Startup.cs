using System.Reflection;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Bus.InProc;
using CloudNativeKit.Infrastructure.Bus.InterProc.Redis;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using CloudNativeKit.Infrastructure.ValidationModel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.Search.ProcessingServices;

namespace VND.CoolStore.Search
{
    public static class Startup
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<ScopedProcessingService>(typeof(IValidator<>));

            //services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher<MessagingDataContext>>();

            services.Configure<RedisOptions>(config.GetSection("Redis"));
            services.AddScoped<RedisStore>();
            services.AddScoped<IMessageBus, RedisMessageBus>();

            services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

            return services;
        }
    }
}
