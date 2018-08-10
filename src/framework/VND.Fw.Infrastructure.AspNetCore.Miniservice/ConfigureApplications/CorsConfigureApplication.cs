using Microsoft.AspNetCore.Builder;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice.ConfigureApplications
{
  public class CorsConfigureApplication : IConfigureApplication
  {
    public int Priority { get; } = 600;
    public void Configure(IApplicationBuilder app)
    {
      app.UseCors("CorsPolicy");
    }
  }
}