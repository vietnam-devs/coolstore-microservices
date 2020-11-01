using System.Collections.Generic;
using N8T.Domain;
using ProductCatalogService.Domain.Dto;

namespace ProductCatalogService.Domain.Event
{
    public class ProductListReplicationEvent : DomainEventBase
    {
        public ProductListReplicationEvent(List<ProductDto> products)
        {
            Products = products;
        }

        public List<ProductDto> Products { get; set; }
    }
}
