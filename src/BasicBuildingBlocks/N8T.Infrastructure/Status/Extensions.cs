using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace N8T.Infrastructure.Status
{
    public static class Extensions
    {
        public static string BuildAppStatus(this IConfiguration config)
        {
            return JsonSerializer.Serialize(config.BuildAppStatusModel());
        }

        public static StatusModel BuildAppStatusModel(this IConfiguration config)
        {
            var model = new StatusModel
            {
                AppName = PlatformServices.Default.Application.ApplicationName,
                AppVersion = PlatformServices.Default.Application.ApplicationVersion,
                BasePath = PlatformServices.Default.Application.ApplicationBasePath
            };

            foreach (var env in config.GetChildren()) model.Envs.Add(env.Key, env.Value);

            model.OsArchitecture = !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    ? "Linux or OSX"
                    : "Others"
                : "Windows";

            model.OsDescription = RuntimeInformation.OSDescription;

            model.ProcessArchitecture = RuntimeInformation.ProcessArchitecture == Architecture.Arm
                ? "Arm"
                : RuntimeInformation.ProcessArchitecture == Architecture.Arm64
                    ? "Arm64"
                    : RuntimeInformation.ProcessArchitecture == Architecture.X64
                        ? "x64"
                        : RuntimeInformation.ProcessArchitecture == Architecture.X86
                            ? "x86"
                            : "Others";

            model.RuntimeFramework = PlatformServices.Default.Application.RuntimeFramework.ToString();
            model.FrameworkDescription = RuntimeInformation.FrameworkDescription;

            model.HostName = Dns.GetHostName();
            model.IpAddress = Dns.GetHostAddresses(Dns.GetHostName())
                .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Aggregate(" ", (a, b) => $"{a} {b}");

            return model;
        }
    }
}
