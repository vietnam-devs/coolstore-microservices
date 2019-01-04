using System;
using NetCoreKit.Domain;

namespace VND.CoolStore.Services.Review.Domain
{
    public class ReviewAuthor : IdentityBase
    {
        private ReviewAuthor()
        {
        }

        public ReviewAuthor(Guid userId)
            : this(userId, "csuser")
        {
        }

        public ReviewAuthor(Guid userId, string username)
        {
            Id = userId;
            UserName = username;
        }

        public string UserName { get; }
    }
}
