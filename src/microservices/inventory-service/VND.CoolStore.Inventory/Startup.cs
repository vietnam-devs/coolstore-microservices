using System.Reflection;
using CloudNativeKit.Infrastructure;
using CloudNativeKit.Infrastructure.Data.Dapper;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Infrastructure.ValidationModel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VND.CoolStore.Inventory
{
    public static class Startup
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services, IConfiguration config)
        {
            var dbOptions = new EfDbOptions();
            config.Bind("ConnectionStrings", dbOptions);

            //services.AddDbContext<MessagingDataContext>(options => options.UseSqlServer(dbOptions.MainDb));
            //services.AddScoped<IEfUnitOfWork<MessagingDataContext>, EfUnitOfWork<MessagingDataContext>>();

            services.Configure<DapperDbOptions>(config.GetSection("ConnectionStrings"));
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();

            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<Domain.Inventory>(typeof(IValidator<>));
            //services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher<MessagingDataContext>>();

            //services.Configure<RedisOptions>(config.GetSection("Redis"));
            //services.AddScoped<RedisStore>();
            //services.AddScoped<IMessageBus, RedisMessageBus>();

            //services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

            return services;
        }
    }
}
