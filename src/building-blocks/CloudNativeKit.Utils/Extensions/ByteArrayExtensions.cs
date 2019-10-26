using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CloudNativeKit.Utils.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBase64String(this byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static string ToUrlSuitable(this byte[] input)
        {
            return input.ToBase64String().Replace("+", "-").Replace("/", "_").Replace("=", "%3d");
        }

        public static string ToHexString(this byte[] bytes)
        {
            var hex = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes) hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        /// <summary>
        /// Ref https://stackoverflow.com/questions/4865104/convert-any-object-to-a-byte
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[]? ToByteArray(this object obj)
        {
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
}
