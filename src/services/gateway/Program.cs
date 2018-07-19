using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace VND.CoolStore.Services.ApiGateway
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Console.Title = "Coolstore - Aggreation Gateway API";
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
  }
}
