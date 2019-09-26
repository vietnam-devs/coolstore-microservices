using System;
using System.Diagnostics;

namespace CloudNativeKit.Utils.Helpers
{
    public static class IdHelper
    {
        [DebuggerStepThrough]
        public static Guid NewId(string guid = "")
        {
            return string.IsNullOrEmpty(guid) ? Guid.NewGuid() : new Guid(guid);
        }

        [DebuggerStepThrough]
        public static Guid EmptyId()
        {
            return Guid.Empty;
        }
    }
}
