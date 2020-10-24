using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N8T.Infrastructure.Logging;
using N8T.Infrastructure.Tye;
using N8T.Infrastructure.Validator;
using Path = System.IO.Path;

namespace N8T.Infrastructure
{
    public static class Extensions
    {
        // [DebuggerStepThrough]
        // public static (WebApplicationBuilder, IConfiguration) AddCustomConfiguration(
        //     this WebApplicationBuilder builder)
        // {
        //     var env = builder.Environment;
        //
        //     var configBuilder = builder.Configuration
        //         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        //         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
        //         .AddEnvironmentVariables();
        //
        //     configBuilder.AddTyeBindingSecrets();
        //
        //     return (builder, configBuilder.Build());
        // }

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
        public static TData ReadData<TData>(this string fileName, string rootFolder)
        {
            var seedData = Path.GetFullPath(fileName, rootFolder);
            Console.WriteLine(seedData);
            using var sr = new StreamReader(seedData);
            var readData = sr.ReadToEnd();
            var models = JsonSerializer.Deserialize<TData>(
                readData,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
            return models;
        }

        [DebuggerStepThrough]
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);
            return model;
        }
    }
}
