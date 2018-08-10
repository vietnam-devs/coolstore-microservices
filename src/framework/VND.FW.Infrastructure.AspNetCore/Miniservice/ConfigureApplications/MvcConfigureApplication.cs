using Microsoft.AspNetCore.Builder;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice
{
  public class MvcConfigureApplication : IConfigureApplication
  {
    public int Priority { get; } = 800;
    public void Configure(IApplicationBuilder app)
    {
      app.UseMvc();
    }
  }
}