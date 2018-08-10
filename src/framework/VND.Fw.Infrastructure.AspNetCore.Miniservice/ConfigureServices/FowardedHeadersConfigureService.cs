using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace VND.Fw.Infrastructure.AspNetCore.Miniservice.ConfigureServices
{
  public class FowardedHeadersConfigureService : IBasicConfigureServices
  {
    public int Priority { get; } = 1000;

    public void Configure(IServiceCollection services)
    {
      var svcProvider = services.BuildServiceProvider();
      var env = svcProvider.GetRequiredService<IHostingEnvironment>();

      if (!env.IsDevelopment())
      {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
          options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
      }
    }
  }
}
