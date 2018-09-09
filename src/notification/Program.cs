using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreKit.Infrastructure.Bus.Kafka;

namespace VND.CoolStore.Services.Notification
{
  public class Program
  {
    public static IConfiguration Configuration { get; set; }

    public static async Task Main(string[] args)
    {
      var host = new HostBuilder()
        .ConfigureHostConfiguration(configHost =>
        {
          configHost.SetBasePath(Directory.GetCurrentDirectory());
          configHost.AddJsonFile("hostsettings.json", optional: true);
          configHost.AddEnvironmentVariables(prefix: "PREFIX_");
          configHost.AddCommandLine(args);
        })
        .ConfigureAppConfiguration((hostContext, configApp) =>
        {
          configApp.AddJsonFile("appsettings.json", optional: true);
          configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
          configApp.AddEnvironmentVariables(prefix: "PREFIX_");
          configApp.AddCommandLine(args);
        })
        .ConfigureServices((hostContext, services) =>
        {
          services.AddMediatR();
          services.AddKafkaEventBus();//TODO: consider to add many topics
          Mapper.Initialize(cfg => cfg.AddProfiles(typeof(Program)));

          var config = services.BuildServiceProvider().GetService<IConfiguration>();

          services.AddLogging();
          services.Configure<KafkaOptions>(config);
          services.AddHostedService<EventsHostedService>();
        })
        .ConfigureLogging((hostContext, configLogging) =>
        {
          configLogging.AddConsole();
          configLogging.AddDebug();
        })
        .UseConsoleLifetime()
        .Build();

      await host.RunAsync();
    }
  }
}
