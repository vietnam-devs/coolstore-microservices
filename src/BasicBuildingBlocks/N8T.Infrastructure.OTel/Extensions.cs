using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N8T.Infrastructure.OTel.MediatR;
using OpenTelemetry.Exporter.Zipkin;
using OpenTelemetry.Trace;

namespace N8T.Infrastructure.OTel
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomOtelWithZipkin(this IServiceCollection services,
            IConfiguration config, Action<ZipkinExporterOptions> configureZipkin = null)
        {
            services.AddOpenTelemetryTracing(b => b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddSqlClientInstrumentation()
                .AddMediatRInstrumentation()
                .AddZipkinExporter(o =>
                {
                    config.Bind("OtelZipkin", o);
                    configureZipkin?.Invoke(o);
                })
            );

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(OTelMediatRTracingBehavior<,>));

            return services;
        }

        private static TracerProviderBuilder AddMediatRInstrumentation(
            this TracerProviderBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder
                .AddInstrumentation(activitySourceAdapter => new OTelMediatRInstrumentation(activitySourceAdapter))
                .AddSource(OTelMediatROptions.OTelMediatRName);

            return builder;
        }
    }
}
