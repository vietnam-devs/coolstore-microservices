using System;
using NetCoreKit.Domain;
using static NetCoreKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.Services.Review.Domain
{
    public class ReviewProduct : IdentityBase
    {
        private ReviewProduct()
        {
        }

        internal ReviewProduct(Guid id)
            : this(id, string.Empty, 0.0D, string.Empty)
        {
        }

        internal ReviewProduct(string name, double price, string desc)
            : this(GenerateId(), name, price, desc)
        {
        }

        internal ReviewProduct(Guid id, string name, double price, string desc)
        {
            Id = id;
            Name = name;
            Price = price;
            Desc = desc;
        }

        public string Name { get; }

        public double Price { get; }

        public string Desc { get; }
    }
}
