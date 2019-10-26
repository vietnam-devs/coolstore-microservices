using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CloudNativeKit.Utils.Extensions
{
    public static class HashExtensions
    {
        private static readonly byte[] _empty = new byte[0];

        public static int CombineHashCodes(this IEnumerable<object> objs)
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in objs) hash = hash * 23 + (obj?.GetHashCode() ?? 0);

                return hash;
            }
        }

        public static byte[] Hash(this string plainText, HashAlgorithm hashAlgorithm, string encoding = "gb2312")
        {
            // get bytes from the plaintext
            var bytes = Encoding.GetEncoding(encoding).GetBytes(plainText);

            // encrypt
            using (var algorithm = hashAlgorithm ?? System.Security.Cryptography.MD5.Create())
            {
                return algorithm.ComputeHash(bytes);
            }
        }

        public static byte[] MD5(this string input, string encoding = "gb2312")
        {
            if (string.IsNullOrEmpty(input)) return _empty;

            return Hash(input, System.Security.Cryptography.MD5.Create(), encoding);
        }

        public static byte[] Sha512(this string input, string encoding = "gb2312")
        {
            if (string.IsNullOrEmpty(input)) return _empty;

            return Hash(input, SHA512.Create(), encoding);
        }

        public static byte[] Sha256(this string input, string encoding = "gb2312")
        {
            if (string.IsNullOrEmpty(input)) return _empty;

            return Hash(input, SHA256.Create());
        }

        public static byte[] Sha1(this string input, string encoding = "gb2312")
        {
            if (string.IsNullOrEmpty(input)) return _empty;

            return Hash(input, SHA1.Create());
        }

        public static byte[]? Sha256(this byte[] input)
        {
            if (input == null) return null;

            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(input);
            }
        }
    }
}
