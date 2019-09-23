using System.Collections.Generic;

namespace CloudNativeKit.Infrastructure.SysInfo
{
    public class SysInfoModel
    {
        public string OSArchitecture { get; set; }
        public string OSDescription { get; set; }
        public string ProcessArchitecture { get; set; }
        public string BasePath { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string AssemplyVersion { get; set; }
        public string RuntimeFramework { get; set; }
        public string FrameworkDescription { get; set; }
        public Dictionary<string, object> Envs { get; set; }
    }
}
