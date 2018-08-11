using System;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreKit.Domain;

namespace VND.CoolStore.Services.Review.Domain
{
  public class ReviewAuthor : IdentityBase
  {
    internal ReviewAuthor() : base()
    {
    }

    public ReviewAuthor(Guid userId)
      : this(userId, "coolstoreuser")
    {
    }

    public ReviewAuthor(Guid userId, string username)
    {
      Id = userId;
      UserName = username;
    }

    [NotMapped]
    public string UserName { get; private set; }
  }
}
