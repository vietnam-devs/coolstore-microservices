using System;
using System.Collections.Generic;
using System.Linq;

namespace VND.Fw.Utils.Helpers
{
    public class EnumHelper
    {
        public static IEnumerable<KeyValueObject<TKey>> GetEnumKeyValue<TEnum, TKey>()
        {
            var metas = GetMetadata<TEnum, TKey>();
            var results = metas.Item1.Zip(metas.Item2, (key, value) =>
                new KeyValueObject<TKey>
                (
                    key: key,
                    value: value
                )
            );
            return results;
        }

        public static (IEnumerable<TKey>, IEnumerable<string>) GetMetadata<TEnum, TKey>()
        {
            var keyArray = (TKey[])Enum.GetValues(typeof(TEnum));
            var nameArray = Enum.GetNames(typeof(TEnum));

            IList<TKey> keys = new List<TKey>();
            foreach (TKey item in keyArray)
            {
                keys.Add(item);
            }

            IList<string> names = new List<string>();
            foreach (var item in nameArray)
            {
                names.Add(item);
            }

            return (keys, names);
        }
    }
}
