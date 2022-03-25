using System;
using System.Globalization;
using System.Runtime.InteropServices;
using TimeZoneConverter;

namespace N8T.Infrastructure.DateTime
{
    public static class Extensions
    {
        public static readonly System.DateTime UnixEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTime(this System.DateTime date)
        {
            if (date.ToUniversalTime() <= UnixEpoch)
            {
                return 0;
            }

            return Convert.ToInt64((date.ToUniversalTime() - UnixEpoch).TotalMilliseconds);
        }

        public static System.DateTime FromUnixTime(this long unixTime) => UnixEpoch.AddMilliseconds(unixTime);

        public static string AsTimestampString(this System.DateTime value) => value.ToString("yyyyMMddHHmmssffff");

        public static string AsTimestampShortString(this System.DateTime value) => value.ToString("yyyyMMddHHmmss");

        public static System.DateTime StartOfTheDay(this System.DateTime value) => new System.DateTime(value.Year, value.Month, value.Day, 0, 0, 0, 1, DateTimeKind.Utc);
        public static long StartOfTheDay(this long value) => value.FromUnixTime().StartOfTheDay().ToUnixTime();
        public static System.DateTime EndOfTheDay(this System.DateTime value) => value.Date.AddDays(1).AddTicks(-1);
        public static long EndOfTheDay(this long value) => value.FromUnixTime().AddDays(1).AddTicks(-1).ToUnixTime();

        public static DateTimeOffset ToDateTimeOffset(this System.DateTime currentUtc, string timeZoneId)
        {
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTimeOffset offset = TimeZoneInfo.ConvertTime(currentUtc, cet);
            return offset;
        }

        public static System.DateTime ToDateTime(this string text)
        {
            return text.ConvertTo<System.DateTime>();
        }

        public static bool IsFutureDate(this object value)
        {
            return value != null && System.DateTime.Compare(value.ToString().ConvertTo<long>().FromUnixTime(), System.DateTime.UtcNow) > 0;
        }

        public static System.DateTime? ToUtcDateTime(this long? value)
        {
            return value.HasValue ? value.ToString().ConvertTo<long>().FromUnixTime() : (System.DateTime?)null;
        }

        public static string ToUtcShortDateString(this long? value)
        {
            return value?.FromUnixTime().Date.ToShortDateString();
        }

        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static string ToLinuxTimezoneId(this string windowsTimezoneId)
        {
            return TZConvert.WindowsToIana(windowsTimezoneId);
        }

        public static string ToWindowsTimezoneId(this string linuxTimezoneId)
        {
            return TZConvert.IanaToWindows(linuxTimezoneId);
        }

        public static long SubtractMinutes(this long timeStamp, int minutes)
        {
            var currentTime = timeStamp.FromUnixTime();
            return currentTime.AddMinutes(-minutes).ToUnixTime();
        }

        public static long AddMinutes(this long timeStamp, int minutes)
        {
            var currentTime = timeStamp.FromUnixTime();
            return currentTime.AddMinutes(minutes).ToUnixTime();
        }

        public static string ToExportDateTime(this System.DateTime value, string culture)
        {
            var dateTimeFormatInfo = new CultureInfo(culture, false).DateTimeFormat;
            return value.ToString($"{dateTimeFormatInfo.ShortDatePattern} HH:mm");
        }

        public static string ToExportDateTime(this System.DateTime value)
        {
            return value.ToExportDateTime(System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
        }
    }
}