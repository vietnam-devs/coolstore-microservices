using System;

namespace VND.Fw.Domain
{
    public interface IIdentity
    {
        Guid Id { get; }
    }
}