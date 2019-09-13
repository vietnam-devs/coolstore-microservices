using System;

namespace CloudNativeKit.Domain
{
    /// <summary>
    /// Supertype for all Identity types with generic Id
    /// </summary>
    public interface IIdentity<TId>
    {
        TId Id { get; }
    }

    /// <summary>
    /// Source: https://github.com/VaughnVernon/IDDD_Samples_NET
    /// </summary>
    public abstract class IdentityBase<TId> : IEquatable<IdentityBase<TId>>, IIdentity<TId>
    {
        protected IdentityBase(TId id)
        {
            Id = id;
        }

        public bool Equals(IdentityBase<TId> id)
        {
            if (ReferenceEquals(this, id))
                return true;
            return !ReferenceEquals(null, id) && Id.Equals(id.Id);
        }

        // currently for Entity Framework, set must be protected, not private.
        // will be fixed in EF.
        public TId Id { get; protected set; }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as IdentityBase<TId>);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
