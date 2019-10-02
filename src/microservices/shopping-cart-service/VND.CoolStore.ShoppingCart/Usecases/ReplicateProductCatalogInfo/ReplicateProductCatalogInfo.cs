using System;
using MediatR;

namespace VND.CoolStore.ShoppingCart.Usecases.ReplicateProductCatalogInfo
{
    public class ReplicateProductCatalogInfo : IRequest<ReplicateProductCatalogInfoResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }

    public class ReplicateProductCatalogInfoResult
    {
    }
}
