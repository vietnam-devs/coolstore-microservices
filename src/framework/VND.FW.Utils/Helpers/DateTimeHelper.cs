using System;

namespace VND.Fw.Utils.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GenerateDateTime()
        {
            return DateTimeOffset.Now.UtcDateTime;
        }
    }
}