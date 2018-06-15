using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace VND.Fw.Utils.Extensions
{
    /// <summary>
    /// https://stackoverflow.com/questions/37895278/how-to-load-assemblies-located-in-a-folder-in-net-core-console-app
    /// </summary>
    public static class AssemblyExtensions
    {
        public static Assembly FindAssemblyBy(this string assemblyName)
        {
            var deps = DependencyContext.Default;
            var res = deps.CompileLibraries.Where(d => d.Name.Contains(assemblyName)).ToList();
            var assembly = Assembly.Load(new AssemblyName(res.First().Name));
            return assembly;
        }
    }
}
