namespace ShoppingCart.Core.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string InventoryId { get; set; } = default!;
    public string InventoryLocation { get; set; } = default!;
    public string CategoryId { get; set; } = default!;
    public string CategoryName { get; set; } = default!;
}
