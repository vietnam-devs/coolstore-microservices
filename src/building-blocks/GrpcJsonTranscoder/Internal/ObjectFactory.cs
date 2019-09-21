using System;
using System.Linq.Expressions;

namespace GrpcJsonTranscoder.Internal
{
    /// <summary>
    /// Ref at https://stackoverflow.com/questions/46500630/how-to-improve-performance-of-c-sharp-object-mapping-code
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Factory<T>
        where T : new()
    {
        private static readonly Func<T> CreateInstanceFunc =
            Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static T CreateInstance() => CreateInstanceFunc();
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
