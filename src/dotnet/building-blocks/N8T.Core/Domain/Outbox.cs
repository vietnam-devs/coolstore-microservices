using System;
using System.ComponentModel.DataAnnotations;

namespace N8T.Core.Domain
{
    public abstract class Outbox : EntityRootBase
    {
        public new Guid Id { get; set; }
        public string Type { get; set; }
        public string AggregateType { get; set; }
        public Guid AggregateId { get; set; }
        public byte[] Payload { get; set; }

        public bool Validate()
        {
            if (Guid.Empty == Id)
            {
                throw new ValidationException("Id of the Outbox entity couldn't be null.");
            }

            if (string.IsNullOrEmpty(Type))
            {
                throw new ValidationException("Type of the Outbox entity couldn't be null or empty.");
            }

            if (string.IsNullOrEmpty(AggregateType))
            {
                throw new ValidationException("AggregateType of the Outbox entity couldn't be null or empty.");
            }

            if (Guid.Empty == AggregateId)
            {
                throw new ValidationException("AggregateId of the Outbox entity couldn't be null.");
            }

            if (Payload is null)
            {
                throw new ValidationException("Payload of the Outbox entity couldn't be null (should be an Avro format).");
            }

            return true;
        }
    }
}
