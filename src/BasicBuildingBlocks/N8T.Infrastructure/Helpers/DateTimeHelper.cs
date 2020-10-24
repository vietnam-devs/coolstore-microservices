using System;
using System.Diagnostics;

namespace N8T.Infrastructure.Helpers
{
    public static class DateTimeHelper
    {
        [DebuggerStepThrough]
        public static DateTime NewDateTime()
        {
            return DateTimeOffset.Now.UtcDateTime;
        }
    }
}