using System;
using System.ComponentModel;
using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public static int Run(this IHostBuilder hostBuilder, bool isRunOnTye = true)
        {
            try
            {
                hostBuilder.Build().Run();
                return 0;
            }
            catch (Exception exception)
            {
                if (isRunOnTye)
                {
                    Log.Fatal(exception, "Host terminated unexpectedly");
                }

                return 1;
            }
            finally
            {
                if (isRunOnTye)
                {
                    Log.CloseAndFlush();
                }
            }
        }
    }
}
