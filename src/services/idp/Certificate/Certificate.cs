using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace VND.CoolStore.Services.Idp.Certificate
{
	/// <summary>
	/// Reference at https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Identity/Identity.API/Certificate/Certificate.cs
	/// </summary>
	static class Certificate
	{
		public static X509Certificate2 Get()
		{
			var assembly = typeof(Certificate).GetTypeInfo().Assembly;
			var names = assembly.GetManifestResourceNames();

			/***********************************************************************************************
			 *  Please note that here we are using a local certificate only for testing purposes. In a 
			 *  real environment the certificate should be created and stored in a secure way, which is out
			 *  of the scope of this project.
			 **********************************************************************************************/
			using (var stream = assembly.GetManifestResourceStream("VND.CoolStore.Services.Idp.Certificate.coolstore.pfx"))
			{
				return new X509Certificate2(ReadStream(stream), "vietnam");
			}
		}

		private static byte[] ReadStream(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
	}
}
