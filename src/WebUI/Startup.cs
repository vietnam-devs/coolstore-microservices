using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using WebUI.Services;

namespace WebUI
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton(GetConfiguration());
      services.AddSingleton<JsInteropService>();
      services.AddSingleton<AuthorizedService>();
      services.AddSingleton<ItemService>();
      services.AddSingleton<CartService>();
      services.AddSingleton<RatingService>();
      services.AddSingleton<AppState>();
    }

    public void Configure(IBlazorApplicationBuilder app)
    {
      app.AddComponent<App>("app");
    }

    public Configuration GetConfiguration()
    {
      // source: https://github.com/aspnet/Blazor/issues/1152
      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("config.json"))
      using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
      {
        return Json.Deserialize<Configuration>(reader.ReadToEnd());
      }
    }
  }

  public class Configuration
  {
    public string ApiUrl { get; set; }
  }
}
