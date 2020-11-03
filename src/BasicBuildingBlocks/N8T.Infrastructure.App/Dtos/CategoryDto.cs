using System;

namespace N8T.Infrastructure.App.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
