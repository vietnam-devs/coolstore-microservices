using System;
using N8T.Domain;

namespace ProductCatalogService.Domain.Model
{
    public class Category : EntityBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;
    }
}
