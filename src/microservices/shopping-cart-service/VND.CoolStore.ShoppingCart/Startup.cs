using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CloudNativeKit.Infrastructure.Grpc;
using CloudNativeKit.Infrastructure.ValidationModel;
using VND.CoolStore.ShoppingCart.AppService;

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

            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(AppServiceStartupRoot).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddAppServices();
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
                endpoints.MapGrpcService<GrpcServices.ShoppingCartService>();
                endpoints.MapGrpcService<GrpcServices.HealthService>();
            });
        }
    }
}
