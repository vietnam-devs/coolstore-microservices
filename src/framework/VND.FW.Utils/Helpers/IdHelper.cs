using System;

namespace VND.Fw.Utils.Helpers
{
    public static class IdHelper
    {
        public static Guid GenerateId(string guid = "")
        {
            return string.IsNullOrEmpty(guid) ? Guid.NewGuid() : new Guid(guid);
        }
    }
}