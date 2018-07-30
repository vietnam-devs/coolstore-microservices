using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using VND.CoolStore.Services.Cart.Infrastructure.Db;
using VND.FW.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Cart
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build()
            .RegisterDbContext<CartDbContext>();
  }
}
