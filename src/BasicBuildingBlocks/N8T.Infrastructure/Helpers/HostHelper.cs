using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using N8T.Infrastructure.Tye;
using Serilog;

namespace N8T.Infrastructure.Helpers
{
    public class HostHelper
    {
        public static (IHostBuilder, bool) CreateHostBuilder<TStartup>(string[] args) where TStartup : class
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            var configuration = CreateConfiguration(args);

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configBuilder => configBuilder.AddConfiguration(configuration));

            var isRunOnTye = configuration.IsRunOnTye();
            if (isRunOnTye)
            {
                hostBuilder = hostBuilder
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<TStartup>();
                    });
            }
            else
            {
                hostBuilder = hostBuilder
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder
                            .UseSerilog()
                            .UseStartup<TStartup>();
                    });
            }

            return (hostBuilder, isRunOnTye);
        }

        private static IConfiguration CreateConfiguration(string[] args)
        {
            var configuration =
                new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .AddJsonFile("appsettings.json", false, true)
                    .AddEnvironmentVariables()
                    .Build();

            return configuration;
        }
    }
}
