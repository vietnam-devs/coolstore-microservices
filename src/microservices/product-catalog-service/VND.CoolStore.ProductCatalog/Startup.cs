using System.Reflection;
using CloudNativeKit.Infrastructure;
using CloudNativeKit.Infrastructure.Data.Dapper;
using CloudNativeKit.Infrastructure.Data.Dapper.Core;
using CloudNativeKit.Infrastructure.ValidationModel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VND.CoolStore.ProductCatalog.Domain;

namespace VND.CoolStore.ProductCatalog
{
    public static class Startup
    {
        public static IServiceCollection AddServiceComponents(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<Product>(typeof(IValidator<>));

            services.Configure<DapperDbOptions>(config.GetSection("ConnectionStrings"));
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();

            return services;
        }
    }
}
