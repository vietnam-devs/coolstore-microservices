using System;
using System.Linq.Expressions;

namespace CloudNativeKit.Utils
{
    /// <summary>
    /// Ref at https://stackoverflow.com/questions/46500630/how-to-improve-performance-of-c-sharp-object-mapping-code
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ObjectFactory<T>
        where T : new()
    {
        private static readonly Func<T> _createInstanceFunc =
            Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static T CreateInstance() => _createInstanceFunc();
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
