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
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddGrpcClientInstrumentation()
                .AddSqlClientInstrumentation(o => o.SetTextCommandContent = true)
                .AddSource(OTelMediatROptions.OTelMediatRName)
                .AddZipkinExporter(o =>
                {
                    config.Bind("OtelZipkin", o);
                    configureZipkin?.Invoke(o);
                })
                .Build());

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(OTelMediatRTracingBehavior<,>));

            return services;
        }
    }
}
