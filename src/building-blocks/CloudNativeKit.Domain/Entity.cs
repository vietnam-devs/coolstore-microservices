using System;
using System.ComponentModel.DataAnnotations;
using static CloudNativeKit.Utils.Helpers.DateTimeHelper;

namespace CloudNativeKit.Domain
{
    public interface IEntity<TId> : IIdentity<TId>
    {
    }

    /// <inheritdoc />
    /// <summary>
    ///  Source: https://github.com/VaughnVernon/IDDD_Samples_NET
    /// </summary>
    public abstract class EntityBase<TId> : IEntity<TId>
    {
        protected EntityBase(TId id)
        {
            Id = id;
            Created = NewDateTime();
        }

        public DateTime Created { get; protected set; }

        public DateTime? Updated { get; protected set; }

        [Key] public TId Id { get; protected set; }
    }
}
