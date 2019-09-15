using System.Reflection;
using CloudNativeKit.Infrastructure.Bus;
using CloudNativeKit.Infrastructure.Data;
using CloudNativeKit.Infrastructure.Grpc;
using CloudNativeKit.Infrastructure.ValidationModel;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VND.CoolStore.ProductCatalog
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddGrpc(options => {
                options.Interceptors.Add<RequestLoggerInterceptor>();
                options.Interceptors.Add<ExceptionHandleInterceptor>();
                options.EnableDetailedErrors = true;
            });

            services.AddMediatR(Assembly.GetEntryAssembly(), typeof(Startup).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddDomainEventBus();

            services.Scan(s =>
                s.FromAssemblyOf<Startup>()
                    .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services.AddDapperComponents();
            services.AddScoped<Data.IProductRepository, Data.Impl.ProductRepository>();
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
                endpoints.MapControllers();
            });
        }
    }
}
