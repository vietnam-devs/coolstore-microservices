namespace VND.Fw.Utils
{
  public class KeyValueObject<TKey>
  {
    public KeyValueObject(TKey key, string value)
    {
      Key = key;
      Value = value;
    }

    public TKey Key { get; private set; }
    public string Value { get; private set; }
  }
}
