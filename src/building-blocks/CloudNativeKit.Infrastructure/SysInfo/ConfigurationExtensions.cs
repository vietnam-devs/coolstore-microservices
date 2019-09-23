using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace CloudNativeKit.Infrastructure.SysInfo
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Ref http://michaco.net/blog/EnvironmentVariablesAndConfigurationInASPNETCoreApps
        /// </summary>
        /// <param name="dynamicObject"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static SysInfoModel GetSystemInformation(this IConfiguration config)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;

            // the app's name and version
            var appName = PlatformServices.Default.Application.ApplicationName;
            var appVersion = PlatformServices.Default.Application.ApplicationVersion;

            var assemblyVersion = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

            // object with some dotnet runtime version information
            var runtimeFramework = PlatformServices.Default.Application.RuntimeFramework;

            // envs
            var envs = new Dictionary<string, object>();

            foreach (var env in config.GetChildren())
                envs.Add(env.Key, env.Key);

            var model = new SysInfoModel
            {
                OSArchitecture = !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? ((RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    ? "Linux or OSX"
                    : "Others")
                : "Windows",

                OSDescription = RuntimeInformation.OSDescription,

                ProcessArchitecture = RuntimeInformation.ProcessArchitecture == Architecture.Arm
                ? "Arm"
                : RuntimeInformation.ProcessArchitecture == Architecture.Arm64
                    ? "Arm64"
                    : RuntimeInformation.ProcessArchitecture == Architecture.X64
                        ? "x64"
                        : RuntimeInformation.ProcessArchitecture == Architecture.X86
                            ? "x86"
                            : "Others",

                BasePath = basePath,
                AppName = appName,
                AppVersion = appVersion,
                AssemplyVersion = assemblyVersion,
                RuntimeFramework = runtimeFramework.ToString(),
                FrameworkDescription = RuntimeInformation.FrameworkDescription,
                Envs = envs
            };

            return model;
        }
    }
}
