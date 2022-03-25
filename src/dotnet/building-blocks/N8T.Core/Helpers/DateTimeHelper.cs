using System;
using System.Diagnostics;

namespace N8T.Core.Helpers
{
    public static class DateTimeHelper
    {
        [DebuggerStepThrough]
        public static DateTime NewDateTime()
        {
            return ToDateTime(DateTimeOffset.Now.UtcDateTime);
        }

        public static DateTime ToDateTime(this DateTime datetime)
        {
            // https://github.com/npgsql/efcore.pg/issues/2050
            return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
        }
    }
}
