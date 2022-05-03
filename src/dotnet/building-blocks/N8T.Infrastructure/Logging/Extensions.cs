using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting.Elasticsearch;

namespace N8T.Infrastructure.Logging
{
    public static class Extensions
    {
        public static void CreateLoggerConfiguration(this IServiceProvider serviceProvider,
            bool isRunOnTye = true)
        {
            if (isRunOnTye)
                return;

            var httpContext = serviceProvider.GetService<IHttpContextAccessor>();
            var config = serviceProvider.GetService<IConfiguration>();
            var fluentdEnabled = config.GetValue("Logging:FluentdEnabled", false);

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(config, "Logging")
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", config.GetValue<string>("App:Name"))
                .Enrich.WithTraceId(httpContext);

            if (!fluentdEnabled)
            {
                // push it directly to ElasticSearch Url endpoint
                loggerConfig
                    .WriteTo.Elasticsearch(config.GetValue<string>("Logging:EsUrl"))
                    .WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level:u3}] [{TraceId}] {Message:lj}{NewLine}{Exception}");
            }
            else
            {
                loggerConfig.WriteTo.Console(new ElasticsearchJsonFormatter());
            }

            Log.Logger = loggerConfig.CreateLogger();
        }

        internal static LoggerConfiguration WithTraceId(this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration,
            IHttpContextAccessor httpContextAccessor)
        {
            if (loggerEnrichmentConfiguration == null)
                throw new ArgumentNullException(nameof(loggerEnrichmentConfiguration));

            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));

            return loggerEnrichmentConfiguration.With(new TraceIdEnricher(httpContextAccessor));
        }
    }
}
