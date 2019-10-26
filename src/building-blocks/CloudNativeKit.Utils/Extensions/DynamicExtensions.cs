using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace CloudNativeKit.Utils.Extensions
{
    public static class DynamicExtensions
    {
        public static dynamic? ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
    }
}
