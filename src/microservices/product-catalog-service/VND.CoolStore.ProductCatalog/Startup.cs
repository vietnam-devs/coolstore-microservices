using System.Reflection;
using CloudNativeKit.Domain;
using CloudNativeKit.Infrastructure;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Bus.InProc;
using CloudNativeKit.Infrastructure.Bus.InterProc.Redis;
using CloudNativeKit.Infrastructure.Bus.Messaging;
using CloudNativeKit.Infrastructure.Data.Dapper;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Infrastructure.Data.EfCore.Core;
using CloudNativeKit.Infrastructure.ValidationModel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.ProductCatalog.Domain;
using VND.CoolStore.ProductCatalog.Gateways;
using VND.CoolStore.ProductCatalog.ProcessingServices;

namespace VND.CoolStore.ProductCatalog
{
    public static class Startup
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services, IConfiguration config)
        {
            var dbOptions = new EfDbOptions();
            config.Bind("ConnectionStrings", dbOptions);

            services.AddDbContext<MessagingDataContext>(options => options.UseSqlServer(dbOptions.MainDb));
            services.AddScoped<IEfUnitOfWork<MessagingDataContext>, EfUnitOfWork<MessagingDataContext>>();

            services.Configure<DapperDbOptions>(config.GetSection("ConnectionStrings"));
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();
            
            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<Product>(typeof(IValidator<>));
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher<MessagingDataContext>>();

            services.Configure<RedisOptions>(config.GetSection("Redis"));
            services.AddScoped<RedisStore>();
            services.AddScoped<IMessageBus, RedisMessageBus>();

            services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
            services.AddScoped<IInventoryGateway, InventoryGateway>();

            return services;
        }
    }
}
