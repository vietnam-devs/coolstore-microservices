using Microsoft.AspNetCore.Builder;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice
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