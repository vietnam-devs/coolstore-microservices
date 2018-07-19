using System;
using VND.Fw.Utils.Helpers;

namespace VND.Fw.Domain
{
  public abstract class IdentityBase : IEquatable<IdentityBase>, IIdentity
  {
    protected IdentityBase()
    {
      Id = IdHelper.GenerateId();
    }

    protected IdentityBase(Guid id)
    {
      Id = id;
    }

    // currently for Entity Framework, set must be protected, not private.
    // will be fixed in EF 6.
    public Guid Id { get; protected set; }

    public bool Equals(IdentityBase id)
    {
      if (ReferenceEquals(this, id)) return true;
      if (ReferenceEquals(null, id)) return false;
      return Id.Equals(id.Id);
    }

    public override bool Equals(object anotherObject)
    {
      return Equals(anotherObject as IdentityBase);
    }

    public override int GetHashCode()
    {
      return GetType().GetHashCode() * 907 + Id.GetHashCode();
    }

    public override string ToString()
    {
      return GetType().Name + " [Id=" + Id + "]";
    }
  }
}
