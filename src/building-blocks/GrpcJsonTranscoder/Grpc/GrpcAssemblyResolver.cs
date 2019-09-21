using Google.Protobuf.Reflection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GrpcJsonTranscoder.Grpc
{
    public class GrpcAssemblyResolver
    {
        private readonly IList<Assembly> _assemblies = new List<Assembly>();
        private ConcurrentDictionary<string, MethodDescriptor> _methodDescriptorDic;

        public GrpcAssemblyResolver ConfigGrpcAssembly(params Assembly[] assemblies)
        {
            if (assemblies != null)
            {
                foreach (var assembly in assemblies)
                {
                    _assemblies.Add(assembly);
                }
            };

            _methodDescriptorDic = GetMethodDescriptors(_assemblies.ToArray());

            return this;
        }

        public MethodDescriptor FindMethodDescriptor(string methodName)
        {
            if (!_methodDescriptorDic.TryGetValue(methodName, out MethodDescriptor methodDescriptor))
            {
                throw new System.Exception($"Could not find out method #{methodName} in the assemblies you provided.");
            }

            return methodDescriptor;
        }

        private ConcurrentDictionary<string, MethodDescriptor> GetMethodDescriptors(params Assembly[] assemblies)
        {
            var methodDescriptorDic = new ConcurrentDictionary<string, MethodDescriptor>();
            var types = assemblies.SelectMany(a => a.GetTypes());
            var fileTypes = types.Where(type => type.Name.EndsWith("Reflection"));

            foreach (var type in fileTypes)
            {
                var flags = BindingFlags.Static | BindingFlags.Public;
                var property = type.GetProperties(flags).Where(t => t.Name == "Descriptor").FirstOrDefault();

                if (property is null) continue;
                if (!(property.GetValue(null) is FileDescriptor fileDescriptor)) continue;

                foreach (var svr in fileDescriptor.Services)
                {
                    var srvName = svr.FullName.ToUpper();
                    foreach (var method in svr.Methods)
                    {
                        methodDescriptorDic.TryAdd(method.Name.ToUpper(), method);
                    }
                }
            }

            return methodDescriptorDic;
        }
    }
}
