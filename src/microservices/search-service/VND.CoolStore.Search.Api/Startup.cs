using System.Security.Claims;
using CloudNativeKit.Infrastructure.Grpc;
using CloudNativeKit.Infrastructure.Tracing.Jaeger;
using CorrelationId;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTracing.Contrib.Grpc.Interceptors;
using VND.CoolStore.Search.Api.Workers;

namespace VND.CoolStore.Search.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationId();
            services.AddJaeger();

            services.AddHealthChecks()
                .AddRedis($"{Configuration["Redis:Host"]},password={Configuration["Redis:Password"]}");

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => Configuration.Bind("JwtAuthn", options));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Name);
                });
            });

            services.AddGrpc(options =>
            {
                options.Interceptors.Add<RequestLoggerInterceptor>();
                options.Interceptors.Add<ExceptionHandleInterceptor>();
                options.Interceptors.Add<ServerTracingInterceptor>();
                options.EnableDetailedErrors = true;
            });

            services.AddServiceComponents(Configuration);
            //services.AddHostedService<OutboxWorker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelationId(new CorrelationIdOptions
            {
                UpdateTraceIdentifier = true
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions {
                    Predicate = _ => true
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions {
                    Predicate = r => r.Name.Contains("self")
                });
                endpoints.MapGrpcService<GrpcServices.ProductSearchService>();
                endpoints.MapControllers();
            });
        }
    }
}
