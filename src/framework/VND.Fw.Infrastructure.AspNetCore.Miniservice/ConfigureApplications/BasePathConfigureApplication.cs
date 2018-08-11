using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VND.Fw.Infrastructure.AspNetCore.Extensions;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice.ConfigureApplications
{
  public class BasePathConfigureApplication : IConfigureApplication
  {
    public int Priority { get; } = 400;
    public void Configure(IApplicationBuilder app)
    {
      var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
      var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
      var logger = loggerFactory.CreateLogger("init");

      var basePath = config.GetBasePath();

      if (!string.IsNullOrEmpty(basePath))
      {
        logger.LogInformation($"Using PATH BASE '{basePath}'");
        app.UsePathBase(basePath);
      }
    }
  }
}
