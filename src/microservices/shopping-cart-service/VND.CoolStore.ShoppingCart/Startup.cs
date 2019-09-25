using System.Reflection;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Bus.InProc;
using CloudNativeKit.Infrastructure.Bus.InterProc.Redis;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using CloudNativeKit.Infrastructure.Data.Dapper;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Infrastructure.ValidationModel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.ShoppingCart.Data;
using VND.CoolStore.ShoppingCart.Domain.Cart;
using VND.CoolStore.ShoppingCart.ProcessingServices;

namespace VND.CoolStore.ShoppingCart
{
    public static class Startup
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services, IConfiguration config)
        {
            var dbOptions = new EfDbOptions();
            config.Bind("ConnectionStrings", dbOptions);

            services.AddDbContext<ShoppingCartDataContext>(options => options.UseSqlServer(dbOptions.MainDb));
            services.AddScoped<IEfUnitOfWork<ShoppingCartDataContext>, EfUnitOfWork<ShoppingCartDataContext>>();

            services.AddDbContext<MessagingDataContext>(options => options.UseSqlServer(dbOptions.MainDb));
            services.AddScoped<IEfUnitOfWork<MessagingDataContext>, EfUnitOfWork<MessagingDataContext>>();

            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher<MessagingDataContext>>();

            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly, typeof(Outbox).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<Cart>(typeof(IValidator<>));

            services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

            services.Configure<RedisOptions>(config.GetSection("Redis"));
            services.AddScoped<RedisStore>();
            services.AddScoped<IMessageBus, RedisMessageBus>();

            return services;
        }
    }
}
