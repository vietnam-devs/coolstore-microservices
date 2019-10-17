namespace CloudNativeKit.Utils
{
    public class KeyValueObject<TKey>
    {
        public KeyValueObject(TKey key, string value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; }
        public string Value { get; }
    }
}
