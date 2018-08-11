using Microsoft.AspNetCore.Builder;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice.ConfigureApplications
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