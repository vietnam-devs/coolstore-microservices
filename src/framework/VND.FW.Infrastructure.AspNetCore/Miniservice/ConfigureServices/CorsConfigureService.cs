using Microsoft.Extensions.DependencyInjection;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice.ConfigureServices
{
  public class CorsConfigureService : IBasicConfigureServices
  {
    public int Priority { get; } = 900;

    public void Configure(IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
          policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
      });
    }
  }
}
