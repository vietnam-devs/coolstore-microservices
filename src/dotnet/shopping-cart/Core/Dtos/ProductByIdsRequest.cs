namespace ShoppingCart.Core.Dtos;

public class ProductByIdsRequest
{
    public List<Guid> ProductIds { get; set; } = new();
}
