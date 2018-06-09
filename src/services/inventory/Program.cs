using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace VND.Services.Inventory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
	            .UseKestrel(options =>
	            {
		            options.ConfigureHttpsDefaults(httpsConfig =>
		            {
			            httpsConfig.CheckCertificateRevocation = false;
		            });
	            })
				.UseStartup<Startup>()
                .Build();
    }
}
