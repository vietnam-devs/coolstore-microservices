using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Cart.Infrastructure.Db;

namespace VND.CoolStore.Services.Cart
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var webHost = BuildWebHost(args);
      if ((webHost.Services.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment).IsDevelopment())
      {
        webHost = webHost.RegisterDbContext<CartDbContext>();
      }
      webHost.Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
          .UseStartup<Startup>()
          .Build();
  }
}
