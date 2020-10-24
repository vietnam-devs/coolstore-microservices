using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace N8T.Infrastructure.Linq
{
    /// <summary>
    /// Copy from https://github.com/arpadbarta/PlatformExtensions
    /// </summary>
    public static class Extensions
    {
        public static void For<T>(this IEnumerable<T> enumerable, Action<int, T> action)
        {
            var collection = enumerable.ToArray();

            for (var i = 0; i < collection.Count(); i++)
            {
                action(i, collection[i]);
            }
        }

        public static IEnumerable<TR> SelectWithIndex<T, TR>(this IEnumerable<T> enumerable, Func<int, T, TR> function)
        {
            var collection = enumerable.ToArray();

            for (var i = 0; i < collection.Count(); i++)
            {
                yield return function(i, collection[i]);
            }
        }

        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var result in enumerable)
            {
                action(result);
            }
        }


        public static IEnumerable<TR> SelectTypeOf<TR>(this IEnumerable enumerable)
        {
            foreach (var element in enumerable)
            {
                if (element is TR typedElement)
                {
                    yield return typedElement;
                }
            }
        }

        public static IEnumerable<TR> SelectTypeOf<T, TR>(this IEnumerable<T> enumerable)
        {
            foreach (var element in enumerable)
            {
                if (element is TR typedElement)
                {
                    yield return typedElement;
                }
            }
        }
    }
}
