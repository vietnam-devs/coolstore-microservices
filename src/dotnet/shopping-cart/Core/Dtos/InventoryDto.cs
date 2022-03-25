namespace ShoppingCart.Core.Dtos;

public class InventoryDto
{
    public Guid Id { get; set; }
    public string Location { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string Description { get; set; } = default!;
}
