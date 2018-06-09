using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;

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
					options.Listen(IPAddress.Any, 5000, listenOptions =>
					{
						var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions()
						{
							ServerCertificate = new X509Certificate2("coolstore.pfx", "vietnam"),
							ClientCertificateMode = ClientCertificateMode.AllowCertificate,
							CheckCertificateRevocation = false,
							ClientCertificateValidation = (certificate2, chain, arg3) => true
						};

						listenOptions.UseHttps(httpsConnectionAdapterOptions);
					});
				})
				.UseStartup<Startup>()
				.Build();
	}
}
