using System;
using System.Collections.Generic;
using N8T.Domain;
using N8T.Infrastructure.App.Dtos;

namespace N8T.Infrastructure.App.Events.ProductCatalog
{
    public class ProductListReplicated : IntegrationEventBase
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public List<Guid> ProductIds { get; set; } = new List<Guid>();
    }
}
