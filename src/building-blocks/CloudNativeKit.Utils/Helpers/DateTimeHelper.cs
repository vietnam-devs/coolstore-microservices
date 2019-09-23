using System;

namespace CloudNativeKit.Utils.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime NewDateTime()
        {
            return DateTimeOffset.Now.UtcDateTime;
        }
    }
}
