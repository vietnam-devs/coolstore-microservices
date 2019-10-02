using System;
using MediatR;

namespace VND.CoolStore.ShoppingCart.Usecases.MarkProductCatalogAsDeleted
{
    public class MarkProductCatalogAsDeleted : IRequest<MarkProductCatalogAsDeletedResult>
    {
        public Guid ProductId { get; set; }
    }

    public class MarkProductCatalogAsDeletedResult
    {
    }
}
