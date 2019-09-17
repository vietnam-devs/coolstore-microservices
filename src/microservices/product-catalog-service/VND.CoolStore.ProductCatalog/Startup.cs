using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VND.CoolStore.ProductCatalog
{
    using CloudNativeKit.Infrastructure;
    using CloudNativeKit.Infrastructure.Bus;
    using CloudNativeKit.Infrastructure.Data;
    using CloudNativeKit.Infrastructure.Grpc;
    using CloudNativeKit.Infrastructure.ValidationModel;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();

            services.AddGrpc(options => {
                options.Interceptors.Add<RequestLoggerInterceptor>();
                options.Interceptors.Add<ExceptionHandleInterceptor>();
                options.EnableDetailedErrors = true;
            });

            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddServiceByIntefaceInAssembly<Startup>(typeof(IValidator<>));

            services.AddDapperComponents();

            services.AddDomainEventBus();
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
                endpoints.MapGrpcService<GrpcServices.CatalogService>();
                endpoints.MapGrpcService<GrpcServices.HealthService>();
                //endpoints.MapControllers();
            });
        }
    }
}
