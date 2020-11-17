using System;
using System.ComponentModel;
using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N8T.Infrastructure.Logging;
using N8T.Infrastructure.Validator;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace N8T.Infrastructure
{
    public static class Extensions
    {
        [DebuggerStepThrough]
        public static IServiceCollection AddCustomMediatR<TType>(this IServiceCollection services,
            Action<IServiceCollection> doMoreActions = null)
        {
            services.AddMediatR(typeof(TType))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            doMoreActions?.Invoke(services);

            return services;
        }

        [DebuggerStepThrough]
        public static IServiceCollection AddCustomMvc<TType>(this IServiceCollection services,
            bool withDapr = false,
            Action<IServiceCollection> doMoreActions = null)
        {
            var mvcBuilder = services.AddControllers();

            if (withDapr)
            {
                mvcBuilder.AddDapr();
            }

            mvcBuilder.AddApplicationPart(typeof(TType).Assembly);

            doMoreActions?.Invoke(services);

            return services;
        }

        [DebuggerStepThrough]
        public static string GetTraceId(this IHttpContextAccessor httpContextAccessor)
        {
            return Activity.Current?.TraceId.ToString() ?? httpContextAccessor?.HttpContext?.TraceIdentifier;
        }

        [DebuggerStepThrough]
        public static T ConvertTo<T>(this object input)
        {
            return ConvertTo<T>(input.ToString());
        }

        [DebuggerStepThrough]
        public static T ConvertTo<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T) converter.ConvertFromString(input);
            }
            catch (NotSupportedException)
            {
                return default;
            }
        }

        [DebuggerStepThrough]
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);
            return model;
        }

        [DebuggerStepThrough]
        public static ILogger ConfigureLogger(this ILogger logger)
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            return logger;
        }
    }
}
