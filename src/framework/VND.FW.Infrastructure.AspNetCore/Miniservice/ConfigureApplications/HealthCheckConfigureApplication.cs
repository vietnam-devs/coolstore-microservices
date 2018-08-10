using Microsoft.AspNetCore.Builder;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice
{
  public class HealthCheckConfigureApplication : IConfigureApplication
  {
    public int Priority { get; } = 300;
    public void Configure(IApplicationBuilder app)
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      app.Map("/liveness", lapp => lapp.Run(async ctx => ctx.Response.StatusCode = 200));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
  }
}