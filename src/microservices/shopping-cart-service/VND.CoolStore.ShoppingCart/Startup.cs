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
using VND.CoolStore.ShoppingCart.Data;
using VND.CoolStore.ShoppingCart.Data.Services;
using VND.CoolStore.ShoppingCart.Domain;
using VND.CoolStore.ShoppingCart.Domain.Cart;
using VND.CoolStore.ShoppingCart.Domain.ProductCatalog;
using VND.CoolStore.ShoppingCart.Gateways;
using VND.CoolStore.ShoppingCart.ProcessingServices;

namespace VND.CoolStore.ShoppingCart
{
    public static class Startup
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services, IConfiguration config)
        {
            var dbOptions = new EfDbOptions();
            config.Bind("ConnectionStrings", dbOptions);

            services.AddDbContext<ShoppingCartDataContext>(options =>
                options.UseSqlServer(dbOptions.MainDb)
                    .EnableSensitiveDataLogging() // for demo only
                    .EnableDetailedErrors()); // for demo only
            services.AddScoped<IEfUnitOfWork<ShoppingCartDataContext>, EfUnitOfWork<ShoppingCartDataContext>>();

            services.AddDbContext<MessagingDataContext>(options => options.UseSqlServer(dbOptions.MainDb));
            services.AddScoped<IEfUnitOfWork<MessagingDataContext>, EfUnitOfWork<MessagingDataContext>>();

            services.Configure<DapperDbOptions>(config.GetSection("ConnectionStrings"));
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();

            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly, typeof(Outbox).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<Cart>(typeof(IValidator<>));
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher<MessagingDataContext>>();
            
            services.Configure<RedisOptions>(config.GetSection("Redis"));
            services.AddScoped<RedisStore>();
            services.AddScoped<IMessageBus, RedisMessageBus>();

            services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
            services.AddScoped<IProductCatalogService, ProductCatalogService>();
            services.AddScoped<IPromoGateway, PromoGateway>();
            services.AddScoped<IShippingGateway, ShippingGateway>();
            services.AddScoped<IInventoryGateway, InventoryGateway>();

            return services;
        }
    }
}
