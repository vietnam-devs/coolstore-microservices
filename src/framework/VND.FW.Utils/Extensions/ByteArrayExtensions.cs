using System;
using System.Text;

namespace VND.Fw.Utils.Extensions
{
  public static class ByteArrayExtensions
  {
    public static string ToBase64String(this byte[] input) => Convert.ToBase64String(input);
    public static string ToUrlSuitable(this byte[] input) => input.ToBase64String().Replace("+", "-").Replace("/", "_").Replace("=", "%3d");
    public static string ToHexString(this byte[] bytes)
    {
      StringBuilder hex = new StringBuilder(bytes.Length * 2);
      foreach (byte b in bytes)
      {
        hex.AppendFormat("{0:x2}", b);
      }

      return hex.ToString();
    }
  }
}
