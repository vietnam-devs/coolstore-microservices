using System;

namespace CloudNativeKit.Utils.Helpers
{
    public static class IdHelper
    {
        public static Guid NewId(string guid = "")
        {
            return string.IsNullOrEmpty(guid) ? Guid.NewGuid() : new Guid(guid);
        }
    }
}
