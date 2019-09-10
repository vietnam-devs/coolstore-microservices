using System;
using System.ComponentModel.DataAnnotations;
using static CloudNativeKit.Utils.Helpers.IdHelper;
using static CloudNativeKit.Utils.Helpers.DateTimeHelper;

namespace CloudNativeKit.Domain
{
    public interface IEntity : IEntityWithId<Guid>
    {
    }

    /// <inheritdoc />
    /// <summary>
    ///  Supertype for all Entity types
    /// </summary>
    public interface IEntityWithId<TId> : IIdentityWithId<TId>
    {
    }

    public abstract class EntityBase : EntityWithIdBase<Guid>
    {
        protected EntityBase() : base(GenerateId())
        {
        }

        protected EntityBase(Guid id) : base(id)
        {
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///  Source: https://github.com/VaughnVernon/IDDD_Samples_NET
    /// </summary>
    public abstract class EntityWithIdBase<TId> : IEntityWithId<TId>
    {
        protected EntityWithIdBase(TId id)
        {
            Id = id;
            Created = GenerateDateTime();
        }

        public DateTime Created { get; protected set; }

        public DateTime Updated { get; protected set; }

        [Key] public TId Id { get; protected set; }
    }
}
