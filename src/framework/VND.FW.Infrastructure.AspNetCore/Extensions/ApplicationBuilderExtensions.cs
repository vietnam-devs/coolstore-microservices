using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VND.FW.Infrastructure.AspNetCore.Extensions
{
  public static class ApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseMiniService(this IApplicationBuilder app)
    {
      IConfiguration config = app.ApplicationServices.GetService<IConfiguration>();
      IHostingEnvironment env = app.ApplicationServices.GetService<IHostingEnvironment>();
      ILoggerFactory loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

      loggerFactory.AddConsole(config.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();

      return app;
    }
  }
}
