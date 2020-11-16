using System;
using InventoryService.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N8T.Infrastructure;
using N8T.Infrastructure.Auth;
using N8T.Infrastructure.Dapr;
using N8T.Infrastructure.EfCore;
using N8T.Infrastructure.OTel;
using N8T.Infrastructure.Tye;
using N8T.Infrastructure.Validator;

namespace InventoryService.Api
{
    public class Startup
    {
        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            Config = config;
            Env = env;
        }

        private IConfiguration Config { get; }
        private IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor()
                .AddCustomMediatR<Anchor>()
                .AddCustomValidators<Anchor>()
                .AddCustomDbContext<MainDbContext, Anchor>(Config.GetConnectionString("postgres"))
                .AddCustomDaprClient()
                .AddControllers()
                .AddDapr();

            services.AddHealthChecks()
                .AddNpgSql(Config.GetConnectionString("postgres"));

            services.AddCustomAuth<Anchor>(Config, options =>
            {
                var isRunOnTye = Config.IsRunOnTye("identityservice");

                options.Authority = isRunOnTye
                    ? Config.GetServiceUri("identityservice")?.AbsoluteUri
                    : options.Authority;

                options.Audience = isRunOnTye
                    ? $"{Config.GetServiceUri("identityservice")?.AbsoluteUri.TrimEnd('/')}/resources"
                    : options.Audience;
            });

            services.AddCustomOtelWithZipkin(Config,
                o =>
                {
                    var isRunOnTye = Config.IsRunOnTye("zipkin");
                    
                    o.Endpoint = isRunOnTye
                        ? new Uri($"http://{Config.GetServiceUri("zipkin")?.DnsSafeHost}:9411/api/v2/spans")
                        : o.Endpoint;
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCloudEvents();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions {Predicate = _ => true});
                endpoints.MapHealthChecks("/liveness",
                    new HealthCheckOptions {Predicate = r => r.Name.Contains("self")});

                endpoints.MapControllers();
                endpoints.MapSubscribeHandler();
            });
        }
    }
}
