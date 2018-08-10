using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VND.FW.Infrastructure.AspNetCore.Middlewares;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice
{
  public class LoggerConfigureApplication : IConfigureApplication
  {
    public int Priority { get; } = 100;
    public void Configure(IApplicationBuilder app)
    {
      var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
      var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

      loggerFactory.AddConsole(config.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseMiddleware<LogHandlerMiddleware>();
    }
  }
}