using System.Reflection;
using CloudNativeKit.Infrastructure.Grpc;
using CloudNativeKit.Infrastructure.ValidationModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VND.CoolStore.ShoppingCart.AppService.GetShoppingCartWithProducts;

namespace VND.CoolStore.ShoppingCart
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options => {
                options.Interceptors.Add<RequestLoggerInterceptor>();
                options.Interceptors.Add<ExceptionHandleInterceptor>();
                options.EnableDetailedErrors = true;
            });

            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(GetShoppingCartWithProductsHandler).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.Scan(s =>
                s.FromAssemblyOf<GetShoppingCartWithProductsHandler>()
                    .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<Services.ShoppingCartService>();
                endpoints.MapGrpcService<Services.HealthService>();
            });
        }
    }
}
