using System;
using System.Text;

namespace CloudNativeKit.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string? TrimStart(this string source, string trim,
            StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (source == null) return null;

            var s = source;
            while (s.StartsWith(trim, stringComparison)) s = s.Substring(trim.Length);

            return s;
        }

        public static string TrimAfter(this string source, string trim)
        {
            var index = source.IndexOf(trim, StringComparison.Ordinal);
            if (index > 0) source = source.Substring(0, index);

            return source;
        }

        public static string MySQLEscape(this string value)
        {
            if (!IsQuoting(value)) return value;

            var sb = new StringBuilder();
            foreach (var c in value)
            {
                var charClass = charClassArray[c];
                if (charClass != CharKinds.None) sb.Append("\\");
                sb.Append(c);
            }

            return sb.ToString();
        }

        #region MySQLEscape Helpers

        private enum CharKinds : byte
        {
            None,
            Quote,
            Backslash
        }

        private static readonly string backslashChars = "\u005c\u00a5\u0160\u20a9\u2216\ufe68\uff3c";

        private static readonly string quoteChars =
            "\u0022\u0027\u0060\u00b4\u02b9\u02ba\u02bb\u02bc\u02c8\u02ca\u02cb\u02d9\u0300\u0301\u2018\u2019\u201a\u2032\u2035\u275b\u275c\uff07";

        private static readonly CharKinds[] charClassArray = MakeCharClassArray();

        private static CharKinds[] MakeCharClassArray()
        {
            var a = new CharKinds[65536];
            foreach (var c in backslashChars) a[c] = CharKinds.Backslash;
            foreach (var c in quoteChars) a[c] = CharKinds.Quote;

            return a;
        }

        private static bool IsQuoting(string str)
        {
            foreach (var c in str)
                if (charClassArray[c] != CharKinds.None)
                    return true;

            return false;
        }

        #endregion
    }
}
