using System;

namespace N8T.Core.Domain
{
    public class CoreException : Exception
    {
        public CoreException(string message) : base(message)
        {
        }

        public static CoreException Exception(string message)
        {
            return new(message);
        }

        public static CoreException NullArgument(string arg)
        {
            return new($"{arg} cannot be null");
        }

        public static CoreException InvalidArgument(string arg)
        {
            return new($"{arg} is invalid");
        }

        public static CoreException NotFound(string arg)
        {
            return new($"{arg} was not found");
        }
    }
}
