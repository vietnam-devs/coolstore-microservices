namespace ShoppingCart.Core.Dtos;

public class CartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = default!;
    public double CartItemTotal { get; set; }
    public double CartItemPromoSavings { get; set; }
    public double ShippingTotal { get; set; }
    public double ShippingPromoSavings { get; set; }
    public double CartTotal { get; set; }
    public bool IsCheckOut { get; set; }
    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
}
