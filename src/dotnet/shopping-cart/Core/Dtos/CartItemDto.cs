namespace ShoppingCart.Core.Dtos;

public class CartItemDto
{
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double PromoSavings { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public double ProductPrice { get; set; }
    public string ProductDescription { get; set; } = default!;
    public string ProductImagePath { get; set; } = default!;
    public Guid InventoryId { get; set; }
    public string InventoryLocation { get; set; } = default!;
}
