using System;

namespace CloudNativeKit.Domain
{
    public class CoreException : Exception
    {
        public CoreException(string message)
            : this(message, null)
        {
        }

        public CoreException(string message, Exception innerEx)
            : base(message, innerEx)
        {
        }
    }

    public class DomainException : CoreException
    {
        public DomainException(string message)
            : base(message, null)
        {
        }
    }

    public class ViolateSecurityException : CoreException
    {
        public ViolateSecurityException(string message)
            : base(message, null)
        {
        }
    }

    public class ValidationException : CoreException
    {
        public ValidationException(string message)
            : base(message, null)
        {
        }
    }
}
