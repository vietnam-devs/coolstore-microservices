using System;
using System.Collections.Generic;

namespace N8T.Infrastructure.App.Requests.ProductCatalog
{
    public class ProductByIdsRequest
    {
        public List<Guid> ProductIds { get; set; } = new List<Guid>();
    }
}
