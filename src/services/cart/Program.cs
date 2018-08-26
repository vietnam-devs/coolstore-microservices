using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Cart.Infrastructure.Db;

namespace VND.CoolStore.Services.Cart
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var webHost = CreateWebHostBuilder(args).Build();
      var env = webHost.Services.GetService<IHostingEnvironment>();
      if (env.IsDevelopment())
        webHost = webHost.RegisterDbContext<CartDbContext>();
      webHost.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseDefaultServiceProvider(o => o.ValidateScopes = false);
  }
}
