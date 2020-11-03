using System;
using System.Collections.Generic;

namespace N8T.Infrastructure.App.Requests.Inventory
{
    public class InventoryByIdsRequest
    {
        public IEnumerable<Guid> InventoryIds { get; set; } = new List<Guid>();
    }
}
