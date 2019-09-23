using System;
using System.ComponentModel;

namespace CloudNativeKit.Utils.Extensions
{
    public static class TypeConversionExtensions
    {
        public static T ConvertTo<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFromString(input);
            }
            catch (NotSupportedException)
            {
                return default;
            }
        }
    }
}
