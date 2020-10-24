using System.Collections.Generic;

namespace N8T.Infrastructure.Status
{
    public class StatusModel
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string OSArchitecture { get; set; }
        public string OSDescription { get; set; }
        public string ProcessArchitecture { get; set; }
        public string BasePath { get; set; }
        public string RuntimeFramework { get; set; }
        public string FrameworkDescription { get; set; }
        public string HostName { get; set; }
        public string IPAddress { get; set; }
        public IDictionary<string, object> Envs { get; set; } = new Dictionary<string, object>();
    }
}
