using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VND.Fw.Utils.Extensions
{
  public static class TypeLookupExtensions
  {
    public static IEnumerable<TypeInfo> ResolveModularGenericTypes(this string patterns, Type genericInterfaceType, Type genericType)
    {
      IEnumerable<TypeInfo> assemblies = patterns
                .LoadAssemblyWithPattern()
                .SelectMany(m => m.DefinedTypes);

      Type interfaceWithType = genericInterfaceType.MakeGenericType(genericType);

      return assemblies.Where(x => x.GetInterfaces().Contains(interfaceWithType));
    }

    public static IEnumerable<TypeInfo> ResolveModularTypes(this string patterns, Type interfaceType)
    {
      IEnumerable<TypeInfo> assemblies = patterns
                .LoadAssemblyWithPattern()
                .SelectMany(m => m.DefinedTypes);

      return assemblies.Where(x =>
          interfaceType.IsAssignableFrom(x) &&
          !x.GetTypeInfo().IsAbstract);
    }

    public static IEnumerable<Assembly> LoadAssemblyWithPattern(this string searchPattern)
    {
      HashSet<Assembly> assemblies = new HashSet<Assembly>();
      Regex searchRegex = new Regex(searchPattern, RegexOptions.IgnoreCase);
      List<RuntimeLibrary> moduleAssemblyFiles = DependencyContext
                .Default
                .RuntimeLibraries
                .Where(x => searchRegex.IsMatch(x.Name)).ToList();

      foreach (RuntimeLibrary assemblyFiles in moduleAssemblyFiles)
      {
        assemblies.Add(Assembly.Load(new AssemblyName(assemblyFiles.Name)));
      }

      return assemblies.ToList();
    }
  }
}
