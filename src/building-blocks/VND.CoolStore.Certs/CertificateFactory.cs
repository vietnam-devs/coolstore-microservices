using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Grpc.Core;

namespace VND.CoolStore.Certs
{
    /// <summary>
    /// Ref https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Identity/Identity.API/Certificate/Certificate.cs
    /// </summary>
    public static class CertificateFactory
    {
        public static X509Certificate2 GetServerPfx()
        {
            var assembly = typeof(CertificateFactory).GetTypeInfo().Assembly;

            /***********************************************************************************************
			 *  Please note that here we are using a local certificate only for testing purposes. In a 
			 *  real environment the certificate should be created and stored in a secure way, which is out
			 *  of the scope of this project.
			 **********************************************************************************************/
            using (var serverPfxStream = assembly.GetManifestResourceStream("VND.CoolStore.Certs.server.pfx"))
                return new X509Certificate2(ReadStream(serverPfxStream), "1111");
        }

        public static SslCredentials GetSslCredential()
        {
            var assembly = typeof(CertificateFactory).GetTypeInfo().Assembly;

            /***********************************************************************************************
			 *  Please note that here we are using a local certificate only for testing purposes. In a 
			 *  real environment the certificate should be created and stored in a secure way, which is out
			 *  of the scope of this project.
			 **********************************************************************************************/
            using (var caStream = assembly.GetManifestResourceStream("VND.CoolStore.Certs.ca.crt"))
            {
                using (var clientCrtStream = assembly.GetManifestResourceStream("VND.CoolStore.Certs.client.crt"))
                {
                    using (var clientKeyStream = assembly.GetManifestResourceStream("VND.CoolStore.Certs.client.key"))
                    {
                        return new SslCredentials(
                            ReadStreamString(caStream),
                            new KeyCertificatePair(
                                ReadStreamString(clientCrtStream),
                                ReadStreamString(clientKeyStream)));
                    }
                }

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

        private static string ReadStreamString(Stream input)
        {
            using (var reader = new StreamReader(input))
                return reader.ReadToEnd();
        }
    }
}
