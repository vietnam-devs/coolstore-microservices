using System;

namespace VND.Fw.Domain
{
    public interface IDomainEvent
    {
        int EventVersion { get; set; }
        DateTime OccurredOn { get; set; }
    }
}