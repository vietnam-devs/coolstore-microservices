using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudNativeKit.Utils.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<KeyValueObject<TKey>> GetEnumKeyValue<TEnum, TKey>()
            where TKey : class
        {
            var metas = GetMetadata<TEnum, TKey>();
            var results = metas.Item1.Zip(metas.Item2, (key, value) =>
                new KeyValueObject<TKey>
                (
                    key,
                    value
                )
            );
            return results;
        }

        public static (IEnumerable<TKey>, IEnumerable<string>) GetMetadata<TEnum, TKey>()
        {
            var keyArray = (TKey[])Enum.GetValues(typeof(TEnum));
            var nameArray = Enum.GetNames(typeof(TEnum));

            var keys = new List<TKey>();
            foreach (var item in keyArray) keys.Add(item);

            var names = new List<string>();
            foreach (var item in nameArray) names.Add(item);

            return (keys, names);
        }
    }
}
