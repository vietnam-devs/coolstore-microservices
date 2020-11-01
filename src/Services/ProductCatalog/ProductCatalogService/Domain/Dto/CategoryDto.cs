using System;

namespace ProductCatalogService.Domain.Dto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
