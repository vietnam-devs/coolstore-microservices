using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N8T.Infrastructure.OTel.MediatR;
using OpenTelemetry.Exporter.Zipkin;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Samplers;

namespace N8T.Infrastructure.OTel
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomOtelWithZipkin(this IServiceCollection services,
            IConfiguration config, Action<ZipkinExporterOptions> configureZipkin = null)
        {
            services.AddOpenTelemetry(b => b
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddSqlClientDependencyInstrumentation()
                .AddMediatRInstrumentation()
                .UseZipkinExporter(o =>
                {
                    config.Bind("OtelZipkin", o);
                    configureZipkin?.Invoke(o);
                })
            );

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(OTelMediatRTracingBehavior<,>));

            return services;
        }

        public static TracerProviderBuilder AddMediatRInstrumentation(
            this TracerProviderBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddInstrumentation((activitySource) => new OTelMediatRInstrumentation(activitySource));
            builder.AddActivitySource(OTelMediatROptions.OTelMediatRName);

            return builder;
        }
    }
}
