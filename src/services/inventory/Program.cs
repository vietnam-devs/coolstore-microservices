using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using VND.CoolStore.Services.Inventory.Infrastructure.Db;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Inventory
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var webHost = BuildWebHost(args);
      if((webHost.Services.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment).IsProduction())
      {
        webHost = webHost.RegisterDbContext<InventoryDbContext>();
      }
      webHost.Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseUrls(urls: "http://*:5004")
        .Build();
  }
}
